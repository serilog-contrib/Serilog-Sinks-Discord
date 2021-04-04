using System;
using System.Collections.Generic;
using DiscordWebhook;
using Serilog.Events;
using Serilog.Sinks.Discord.Helpers;

namespace Serilog.Sinks.Discord
{
    public static class WebhookObjectFactory
    {
        public static WebhookObject Create(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var obj = new WebhookObject
            {
                embeds = logEvent.Exception != null
                    ? new Embed[] {BuildExceptionEmbed(logEvent, formatProvider)}
                    : new Embed[] {BuildBasicEmbed(logEvent, formatProvider)}
            };

            return obj;
        }

        private static Embed BuildBasicEmbed(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var message = logEvent.RenderMessage(formatProvider);
            message = message?.Length > 256 ? message.Substring(0, 256) : message;

            var embed = new Embed()
            {
                description = message,
                color = GetColor(logEvent.Level)
            };

            return embed;
        }

        private static Embed BuildExceptionEmbed(LogEvent logEvent, IFormatProvider formatProvider)
        {
            var stackTrace = logEvent.Exception.StackTrace;
            stackTrace = FormatStackTrace(stackTrace);

            var embed = new Embed()
            {
                description = logEvent.Exception.Message,
                color = GetColor(logEvent.Level),
                thumbnail = new Thumbnail() {url = "https://raw.githubusercontent.com/javis86/Serilog.Sinks.Discord/master/Resources/error.png"},
            };

            var fields = new List<Field>()
            {
                new Field() {name = "Type", value = logEvent.Exception.GetType().Name, inline = true},
                new Field() {name = "TimeStamp", value = logEvent.Timestamp.ToString(), inline = true},
                new Field() {name = "Message", value = logEvent.Exception.Message, inline = false},
                new Field() {name = "StackTrace", value = stackTrace, inline = false}
            };
            fields.AddRange(GetFieldsFromEventProperties(logEvent.Properties));
            embed.fields = fields.ToArray();

            return embed;
        }

        private static string FormatStackTrace(string stackTrace)
        {
            if (!string.IsNullOrEmpty(stackTrace)
                && stackTrace.Length >= 1024)
                stackTrace = stackTrace.Substring(0, 1015) + "...";

            stackTrace = $"```{stackTrace ?? "NA"}```";
            return stackTrace;
        }

        private static int GetColor(LogEventLevel level)
        {
            return level switch
            {
                LogEventLevel.Debug => (int) Color.Purple,
                LogEventLevel.Error => (int) Color.Red,
                LogEventLevel.Fatal => (int) Color.DarkRed,
                LogEventLevel.Information => (int) Color.Green,
                LogEventLevel.Verbose => (int) Color.Grey,
                LogEventLevel.Warning => (int) Color.Orange,
                _ => (int) Color.Orange
            };
        }

        public static IEnumerable<Field> GetFieldsFromEventProperties(IReadOnlyDictionary<string, LogEventPropertyValue> logEventProperties)
        {
            var properties = new List<Field>();

            foreach (var property in logEventProperties)
            {
                var value = property.Value.ToString().Length < 1024 
                    ? property.Value.ToString() 
                    : property.Value.ToString().Substring(0, 1020) + "...";

                properties.Add(new Field()
                {
                    name = property.Key,
                    value = value,
                    inline = true
                });
            }
            
            return properties.ToArray();
        }
    }
}