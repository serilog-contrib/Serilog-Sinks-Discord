using Serilog.Configuration;
using Serilog.Events;
using System;

namespace Serilog.Sinks.Discord
{
    public static class DiscordSinkExtenstions
    {
        /// <summary>
        /// Adds a Discord sink to the provided logger configuration.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration to add the sink to.</param>
        /// <param name="webhookId">The webhook ID for the Discord channel.</param>
        /// <param name="webhookToken">The webhook token for the Discord channel.</param>
        /// <param name="restrictedToMinimumLevel">Minimum log event level required to write an event to the sink.</param>
        /// <param name="botName">The Name from which the messages will be send.</param>
        /// <param name="avatarURL">A link to the Avatar URL to be shown.</param>
        /// <param name="formatProvider">Optional format provider.</param>
        /// <returns>Logger configuration, allowing configuration to continue.</returns>
        public static LoggerConfiguration Discord(
            this LoggerSinkConfiguration loggerConfiguration,
            UInt64 webhookId,
            string webhookToken,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Debug,
            string botName = null,
            string avatarURL = null,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration is null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }
            LogEventLevel.
            return loggerConfiguration.Sink(new DiscordSink(formatProvider, webhookId, webhookToken, restrictedToMinimumLevel, botName, avatarURL));
        }
    }
}