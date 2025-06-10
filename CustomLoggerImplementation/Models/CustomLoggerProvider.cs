namespace CustomLoggerImplementation.Models
{
    // CustomLoggerProvider implements the ILoggerProvider interface.
    // ILoggerProvider is part of the ASP.NET Core logging infrastructure,
    // and its main purpose is to create ILogger instances for different categories.
    public class CustomLoggerProvider : ILoggerProvider
    {
        // Private field to hold the file path where log messages will be written.
        private readonly string _logFilePath;

        // Private field that represents the minimum log level required to process a log message.
        private readonly LogLevel _minLogLevel;

        // Private field to store the IServiceScopeFactory.
        // This is used to create a new dependency injection (DI) scope,
        // allowing the logger to resolve scoped services (e.g., DbContext).
        private readonly IServiceScopeFactory _scopeFactory;

        // Constructor for the CustomLoggerProvider.
        // Parameters:
        //   logFilePath: The path to the log file where log messages will be appended.
        //   minimumLogLevel: The minimum log severity level that this provider will log.
        //   scopeFactory: A factory for creating DI scopes, used to resolve scoped services like LoggingDbContext.
        public CustomLoggerProvider(string logFilePath, LogLevel minimumLogLevel, IServiceScopeFactory scopeFactory)
        {
            _logFilePath = logFilePath;         // Assign the provided file path to the private field.

            _minLogLevel = minimumLogLevel;     // Assign the provided minimum log level to the private field.

            _scopeFactory = scopeFactory;       // Assign the provided scope factory to the private field.
        }

        // CreateLogger is called by the logging system to create an ILogger instance for a specific category.
        // Parameter:
        //   categoryName: A string that typically represents the name of the class or category where the logger is used.
        // Returns:
        //   A new instance of CustomLogger configured with the specified category, file path, log level, and scope factory.
        public ILogger CreateLogger(string categoryName)
        {
            // Create and return a new CustomLogger instance using the provided category name and the previously set fields.
            return new CustomLogger(categoryName, _logFilePath, _minLogLevel, _scopeFactory);
        }

        // Dispose method for cleaning up any resources if necessary.
        // In this simple example, there are no resources to dispose of, so the method is left empty.
        public void Dispose() { }

    }
}
