using System;
using System.Collections.Generic;
using System.IO;
using Discord;
using Discord.Webhook;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.Discord
{
    public class DiscordSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        private readonly UInt64 _webhookId;
        private readonly string _webhookToken;

        public DiscordSink(IFormatProvider formatProvider,
            UInt64 webhookId,
            string webhookToken)
        {
            _formatProvider = formatProvider;
            _webhookId = webhookId;
            _webhookToken = webhookToken;
        }

        public void Emit(LogEvent logEvent)
        {
            SendMessage(logEvent);
        }

        private void SendMessage(LogEvent logEvent)
        {
            var webHook = new DiscordWebhookClient(_webhookId, _webhookToken);

            try
            {
                Embed embed;
                if (logEvent.Exception != null)
                {
                    embed = BuildExceptionEmbed(logEvent);
                }
                else
                {
                    embed = BuildBasicEmbed(logEvent);
                }

                webHook.SendMessageAsync(null, false, new List<Embed>() {embed}).Wait();
            }
            catch (Exception ex)
            {
                webHook.SendMessageAsync($"ooo snap, {ex.Message}", false).Wait();
            }
        }

        private Embed BuildBasicEmbed(LogEvent logEvent)
        {
            var embedBuilder = new EmbedBuilder();
            var message = logEvent.RenderMessage(_formatProvider);

            string title =
                message?.Length > 256
                    ? message.Substring(0, 256)
                    : message;

            embedBuilder.Color = GetColor(logEvent.Level);
            embedBuilder.Title = title;
            return embedBuilder.Build();
        }

        private static Embed BuildExceptionEmbed(LogEvent logEvent)
        {
            var embedBuilder = new EmbedBuilder();
            var stackTrace = logEvent.Exception.StackTrace;

            stackTrace = FormatStackTrace(stackTrace);

            embedBuilder.Color = new Color(255, 0, 0);
            embedBuilder.WithTitle("Error")
                .WithDescription(logEvent.Exception.Message)
                //.WithThumbnailUrl($"attachment://error.png")
                .WithThumbnailUrl($"https://raw.githubusercontent.com/javis86/Serilog.Sinks.Discord/master/Resources/error.png")
                .AddField("Type", logEvent.Exception.GetType().Name, true)
                .AddField("TimeStamp", logEvent.Timestamp.ToString(), true)
                .AddField("Message", logEvent.Exception.Message)
                .AddField("StackTrace", stackTrace)
                .AddFieldRange(logEvent.Properties)
                .WithCurrentTimestamp();

            return embedBuilder.Build();
        }

        private static string FormatStackTrace(string stackTrace)
        {
            if (!string.IsNullOrEmpty(stackTrace)
                && stackTrace.Length >= 1024)
                stackTrace = stackTrace.Substring(0, 1015) + "...";

            stackTrace = $"```{stackTrace ?? "NA"}```";
            return stackTrace;
        }

        private static Color GetColor(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Debug:
                    return Color.Purple;

                case LogEventLevel.Error:
                    return Color.Red;

                case LogEventLevel.Fatal:
                    return Color.DarkRed;

                case LogEventLevel.Information:
                    return new Color(0, 186, 255);

                case LogEventLevel.Verbose:
                    return new Color(0, 0, 0);

                case LogEventLevel.Warning:
                    return Color.Orange;

                default:
                    return new Color();
            }
        }
    }
}