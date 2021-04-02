using System.Collections.Generic;
using Discord;
using Serilog.Events;

namespace Serilog.Sinks.Discord
{
    public static class EmbedBuilderExtensions
    {
        public static EmbedBuilder AddFieldRange(this EmbedBuilder builder, IReadOnlyDictionary<string, LogEventPropertyValue> logEventProperties)
        {
            foreach (var property in logEventProperties)
            {
                var value = property.Value.ToString().Length < 1024 
                    ? property.Value.ToString() 
                    : property.Value.ToString().Substring(0, 1020) + "...";
                builder.AddField(property.Key, value, true);
            }
            
            return builder;
        }
    }
}