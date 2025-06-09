namespace CustomLoggerImplementation.Models
{
    public class LogEntry
    {
        public int Id { get; set; }

        // The logger category name
        public string? Category { get; set; }

        // The actual log message 
        public string? Message { get; set; }

        // The log level (e.g. Information, Error, etc.)
        public string LogLevel { get; set; }

        //Optional EventId
        public int? EventId { get; set; }

        // Exception details if any
        public string? Exception { get; set; }

        // The time when the log was created
        public DateTime CreatedTime { get; set; }

    }
}
