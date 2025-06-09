namespace CustomLoggerImplementation.Models
{
    // CustomLogger implements the ILogger interface, which provides a standard logging contract.
    public class CustomLogger : ILogger
    {

        // Private fields to hold configuration values for this logger instance.
        private readonly string _categoryName;         // Holds the logger category name (usually the class name where the logger is used).

        private readonly string _logFilePath;          // Path to the file where log messages will be written.

        private readonly LogLevel _minLogLevel;        // Minimum log level required to write a log (filters out less severe messages).

        private readonly IServiceScopeFactory _scopeFactory;

        public CustomLogger(string categoryName, string logFilePath, LogLevel minLogLevel, IServiceScopeFactory scopeFactory)
        {
            _categoryName = categoryName;

            _logFilePath = logFilePath;

            _minLogLevel = minLogLevel;

            _scopeFactory = scopeFactory;
        }

        // BeginScope creates a logging scope. Not implemented here, so it returns null.
        // Scopes can be used to group a set of logical operations under a common context.
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        // IsEnabled checks if a given log level is enabled based on the minimum log level configured.
        // Only logs messages with a severity equal to or higher than _minLogLevel.
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLogLevel;
        }

        // The Log method writes the log entry.
        // logLevel: The severity of the log (e.g., Information, Warning, Error).
        // eventId: An identifier for the event.
        // TState: This parameter can be any object that contains data to be logged.
        // exception: An optional exception object that provides error details if any, including the stack trace. 
        // formatter: A delegate that takes the state and exception as inputs and returns a formatted log message string.
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Check if the current log level is enabled. If not, exit early.
            if (!IsEnabled(logLevel))
                return;

            // Create the formatted message using the provided formatter function.
            var message = formatter(state, exception);

            // If the resulting message is null or empty, do not log.
            if (string.IsNullOrEmpty(message))
                return;

            // Build a log record string for the file with timestamp, log level, category, and message.
            var logRecord = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {_categoryName}: {message}";

            // Write the log message to the file system, appending a new line.
            File.AppendAllText(_logFilePath, logRecord + Environment.NewLine);

            // Write the log to SQL Server using EF Core.
            // Create a new service scope so that the DbContext is resolved in a scoped manner.
            using (var scope = _scopeFactory.CreateScope())
            {

                // Resolve the LoggingDbContext from the dependency injection container.
                var dbContext = scope.ServiceProvider.GetRequiredService<LoggingDbContext>();

                // Create a new LogEntry entity with the log details.
                var logEntry = new LogEntry
                {
                    Category = _categoryName,               // Store the logger category name.

                    Message = message,                      // Store the formatted log message.

                    EventId = eventId.Id,                   // Store the event ID (if provided).

                    LogLevel = logLevel.ToString(),         // Store the log level as a string.

                    Exception = exception?.ToString(),      // Store exception details if any exist.

                    CreatedTime = DateTime.Now              // Store the time when the log was created.

                };

                // Add the log entry to the database context.
                dbContext.LogEntries.Add(logEntry);

                // Save the changes to persist the log entry to the database.
                dbContext.SaveChanges();

            }

        }

    }
}
