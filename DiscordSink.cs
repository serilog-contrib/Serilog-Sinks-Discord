using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
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

        public DiscordSink(
            IFormatProvider formatProvider,
             UInt64 webhookId,
              string webhookToken)
        {
            _formatProvider = formatProvider;
            _webhookId=webhookId;
            _webhookToken=webhookToken;
        }

        public  void Emit(LogEvent logEvent)
        {
            SendMessageAsync(logEvent);
        }

        private async Task SendMessageAsync(LogEvent logEvent)
        {
            var embedBuilder = new EmbedBuilder();
            var embeds = new List<Embed>();
            var webHook = new DiscordWebhookClient(_webhookId, _webhookToken);

            try
            {
                if (logEvent.Exception != null)
                {
                    string stackTrace = logEvent.Exception.StackTrace;

                    if (!string.IsNullOrEmpty(stackTrace) 
                        && stackTrace.Length > 1024)
                        stackTrace = stackTrace.Substring(0, 1020) + " ...";

                    embedBuilder.Color = new Color(255, 0, 0);
                    embedBuilder.WithTitle("An exception occurred :");
                    embedBuilder.AddField("Type", logEvent.Exception.GetType().Name);
                    embedBuilder.AddField("Message", logEvent.Exception.Message);
                    embedBuilder.AddField("StackTrace", stackTrace);
                    
                    embeds.Add(embedBuilder.Build());

                    await webHook.SendMessageAsync(null, false, embeds);
                }
                else
                {
                    var message = logEvent.RenderMessage(_formatProvider);

                    string title = 
                        message?.Length > 256 
                        ? message.Substring(0, 256) 
                        : message;

                    embedBuilder.Color = GetColor(logEvent.Level);
                    embedBuilder.Title = title;
                    embeds.Add(embedBuilder.Build());

                    await webHook.SendMessageAsync(null, false, embeds);
                }
            }

            catch(Exception ex)
            {
                await webHook.SendMessageAsync(
                    $"ooo snap, {ex.Message}",
                    false);
            }
            
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
                    return new Color(0,0,0);
                
                case LogEventLevel.Warning:
                    return Color.Orange;
                
                default:
                    return new Color();
            }

        }
    }
}
