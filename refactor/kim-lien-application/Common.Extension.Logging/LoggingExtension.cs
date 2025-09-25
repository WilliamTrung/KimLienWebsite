using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Common.Extension.Logging
{
    public static class LoggingExtension
    {
        public static void LogDataInformation(this ILogger logger, string message, object? data = null, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            logger.LogData(LogLevel.Information, message, data, methodName, filePath, lineNumber);
        }
        public static void LogDataWarning(this ILogger logger, string message, object? data = null, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            logger.LogData(LogLevel.Warning, message, data, methodName, filePath, lineNumber);
        }
        public static void LogDataError(this ILogger logger, string message, object? data = null, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            logger.LogData(LogLevel.Error, message, data, methodName, filePath, lineNumber);
        }
        public static void LogData(this ILogger logger, LogLevel logLevel, string message, object? data = null, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var logInfo = new List<string>()
            {
                $"file: {Path.GetFileNameWithoutExtension(filePath)}",
                $"method: {methodName}",
                $"line: {lineNumber}",
                $"message: {message}"
            };

            if (data is not null)
            {
#if !DEBUG
                logInfo.Add("data: {data}");
#else
                logInfo.Add($"data: {data.TrySerializeObject()}");
#endif
            }

            var logMessage = string.Join(", ", logInfo);
#if !DEBUG
            logger.Log(logLevel, logMessage, data?.TrySerializeObject());
#else
            Debug.WriteLine(logMessage);
#endif
        }
    }
}
