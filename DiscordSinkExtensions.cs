using System;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.Sinks.Discord
{

    public static class DiscordSinkExtensions
    {
        public static LoggerConfiguration Discord(
                this LoggerSinkConfiguration loggerConfiguration,
                UInt64 webhookId,
                string webhookToken,
                IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new DiscordSink(formatProvider,
                webhookId,
                webhookToken), LevelAlias.Minimum);
        }
        
        public static LoggerConfiguration Discord(
            this LoggerSinkConfiguration loggerConfiguration,
            UInt64 webhookId,
            string webhookToken,
            LogEventLevel restrictedToMinimumLevel,
            IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new DiscordSink(formatProvider,
                webhookId,
                webhookToken), restrictedToMinimumLevel);
        }
        
   
    }

}