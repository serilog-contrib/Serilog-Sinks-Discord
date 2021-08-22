using System;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.Sinks.Discord
{
    public static class DiscordSinkExtenstions
    {
        public static LoggerConfiguration Discord(
                this LoggerSinkConfiguration loggerConfiguration,
                UInt64 webhookId,
                string webhookToken,
                IFormatProvider formatProvider = null,
                LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose)
        {
            return loggerConfiguration.Sink(
                new DiscordSink(formatProvider, webhookId, webhookToken, restrictedToMinimumLevel));
        }
    }
}