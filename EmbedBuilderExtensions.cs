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
                builder.AddField(property.Key, property.Value.ToString(), true);
            }
            
            return builder;
        }
    }
}