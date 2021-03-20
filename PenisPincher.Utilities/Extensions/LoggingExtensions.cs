using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using PenisPincher.Utilities.JetBrains.Annotations;

namespace PenisPincher.Utilities.Extensions
{
    public static class LoggingExtensions
    {
        #region Trace
        [StringFormatMethod("message")]
        public static void Trace<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Trace, message, args);
        }

        [StringFormatMethod("message")]
        public static void Trace<T>(this ILogger<T> logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Trace, exception, message, args);
        }

        [StringFormatMethod("message")]
        public static void Trace<T>(this ILogger<T> logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Trace, eventId, message, args);
        }

        [StringFormatMethod("message")]
        public static void Trace<T>(this ILogger<T> logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Trace, eventId, exception, message, args);
        }
        #endregion

        #region Debug
        [StringFormatMethod("message")]
        public static void Debug<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Debug, message, args);
        }

        [StringFormatMethod("message")]
        public static void Debug<T>(this ILogger<T> logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Debug, exception, message, args);
        }

        [StringFormatMethod("message")]
        public static void Debug<T>(this ILogger<T> logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Debug, eventId, message, args);
        }

        [StringFormatMethod("message")]
        public static void Debug<T>(this ILogger<T> logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Debug, eventId, exception, message, args);
        }
        #endregion

        #region Information
        [StringFormatMethod("message")]
        public static void Information<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Information, message, args);
        }

        [StringFormatMethod("message")]
        public static void Information<T>(this ILogger<T> logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Information, exception, message, args);
        }

        [StringFormatMethod("message")]
        public static void Information<T>(this ILogger<T> logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Information, eventId, message, args);
        }

        [StringFormatMethod("message")]
        public static void Information<T>(this ILogger<T> logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Information, eventId, exception, message, args);
        }
        #endregion

        #region Warning
        [StringFormatMethod("message")]
        public static void Warning<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Warning, message, args);
        }

        [StringFormatMethod("message")]
        public static void Warning<T>(this ILogger<T> logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Warning, exception, message, args);
        }

        [StringFormatMethod("message")]
        public static void Warning<T>(this ILogger<T> logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Warning, eventId, message, args);
        }

        [StringFormatMethod("message")]
        public static void Warning<T>(this ILogger<T> logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Warning, eventId, exception, message, args);
        }
        #endregion

        #region Error
        [StringFormatMethod("message")]
        public static void Error<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Error, message, args);
        }

        [StringFormatMethod("message")]
        public static void Error<T>(this ILogger<T> logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Error, exception, message, args);
        }

        [StringFormatMethod("message")]
        public static void Error<T>(this ILogger<T> logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Error, eventId, message, args);
        }

        [StringFormatMethod("message")]
        public static void Error<T>(this ILogger<T> logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Error, eventId, exception, message, args);
        }
        #endregion

        #region Critical
        [StringFormatMethod("message")]
        public static void Critical<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.Log(LogLevel.Critical, message, args);
        }

        [StringFormatMethod("message")]
        public static void Critical<T>(this ILogger<T> logger, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Critical, exception, message, args);
        }

        [StringFormatMethod("message")]
        public static void Critical<T>(this ILogger<T> logger, EventId eventId, string message, params object[] args)
        {
            logger.Log(LogLevel.Critical, eventId, message, args);
        }

        [StringFormatMethod("message")]
        public static void Critical<T>(this ILogger<T> logger, EventId eventId, Exception exception, string message, params object[] args)
        {
            logger.Log(LogLevel.Critical, eventId, exception, message, args);
        }
        #endregion
    }
}
