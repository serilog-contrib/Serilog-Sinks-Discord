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
                LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
                IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new DiscordSink(formatProvider,
                                                            webhookId,
                                                            webhookToken), restrictedToMinimumLevel);
        }
        
   
    }

}