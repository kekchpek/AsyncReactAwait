namespace AsyncReactAwait.Logging
{
    /// <summary>
    /// An entry point to handle internal logging.
    /// </summary>
    public static class Logger
    {

        private static ILogger _logger;
        
        /// <summary>
        /// Sets the logger for internal logging.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> implementation.</param>
        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Logs the message with set internal logger.
        /// Use <see cref="SetLogger"/> to set the internal logger.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            _logger?.Log(message);
        }
    }
}