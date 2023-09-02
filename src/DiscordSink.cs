using Discord;
using Discord.Webhook;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace Serilog.Sinks.Discord
{
    /// <summary>
    /// Represents a sink that writes log events to a Discord channel via a webhook.
    /// </summary>
    public class DiscordSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        private readonly UInt64 _webhookId;
        private readonly string _webhookToken;
        private readonly string _botName = null;
        private readonly string _avatarURL = null;
        private readonly LogEventLevel _restrictedToMinimumLevel;
        private readonly DiscordWebhookClient _webHook;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscordSink"/> class.
        /// </summary>
        /// <param name="formatProvider">Provides formatting for output strings.</param>
        /// <param name="webhookId">The webhook ID for the Discord channel.</param>
        /// <param name="webhookToken">The webhook token for the Discord channel.</param>
        /// <param name="restrictedToMinimumLevel">Minimum log event level required to write an event to the sink.</param>
        /// <param name="botName">The name under which messages will be sent. Can be null.</param>
        /// <param name="avatarURL">URL to the avatar to be shown. Can be null.</param>
        public DiscordSink(
            IFormatProvider formatProvider,
            UInt64 webhookId,
            string webhookToken,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Information,
            string botName = null,
            string avatarURL = null)
        {
            _formatProvider = formatProvider;
            _webhookId = webhookId;
            _webhookToken = webhookToken;
            _botName = botName;
            _avatarURL = avatarURL;
            _restrictedToMinimumLevel = restrictedToMinimumLevel;

            _webHook = new DiscordWebhookClient(_webhookId, _webhookToken);
        }
        /// <summary>
        /// Emit the provided log event to the sink.
        /// </summary>
        /// <param name="logEvent">The log event to write.</param>
        public void Emit(LogEvent logEvent)
        {
            SendMessageAsync(logEvent).Wait();
        }
        /// <summary>
        /// Asynchronously sends the log message to the Discord webhook.
        /// </summary>
        /// <param name="logEvent">The log event to send.</param>
        private async Task SendMessageAsync(LogEvent logEvent)
        {
            if (!ShouldLogMessage(_restrictedToMinimumLevel, logEvent.Level))
                return;
            try
            {
                EmbedBuilder embedBuilder = new EmbedBuilder();
                if (logEvent.Exception != null)
                {
                    embedBuilder.Color = new Color(255, 0, 0);
                    embedBuilder.WithTitle(":o: Exception");
                    embedBuilder.AddField("Type:", $"```{logEvent.Exception.GetType().FullName}```");

                    string message = FormatMessage(logEvent.Exception.Message, 1000);
                    embedBuilder.AddField("Message:", message);

                    string stackTrace = FormatMessage(logEvent.Exception.StackTrace, 1000);
                    embedBuilder.AddField("StackTrace:", stackTrace);
                }
                else
                {
                    string message = logEvent.RenderMessage(_formatProvider);

                    message = FormatMessage(message, 240);

                    SpecifyEmbedLevel(logEvent.Level, embedBuilder);

                    embedBuilder.Description = message;
                }
                await _webHook.SendMessageAsync(null, false, new Embed[] { embedBuilder.Build() }, _botName, _avatarURL);
            }
            catch (Exception ex)
            {
                await _webHook.SendMessageAsync($"Error sending Discord Webhook: {ex.Message}", false, null, _botName, _avatarURL);
            }
        }
        /// <summary>
        /// Configures the embed's title and color based on the log event level.
        /// </summary>
        /// <param name="level">The log event level.</param>
        /// <param name="embedBuilder">The embed builder to configure.</param>
        private static void SpecifyEmbedLevel(LogEventLevel level, EmbedBuilder embedBuilder)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    embedBuilder.Title = ":loud_sound: Verbose";
                    embedBuilder.Color = Color.LightGrey;
                    break;
                case LogEventLevel.Debug:
                    embedBuilder.Title = ":mag: Debug";
                    embedBuilder.Color = Color.LightGrey;
                    break;
                case LogEventLevel.Information:
                    embedBuilder.Title = ":information_source: Information";
                    embedBuilder.Color = new Color(0, 186, 255);
                    break;
                case LogEventLevel.Warning:
                    embedBuilder.Title = ":warning: Warning";
                    embedBuilder.Color = new Color(255, 204, 0);
                    break;
                case LogEventLevel.Error:
                    embedBuilder.Title = ":x: Error";
                    embedBuilder.Color = new Color(255, 0, 0);
                    break;
                case LogEventLevel.Fatal:
                    embedBuilder.Title = ":skull_crossbones: Fatal";
                    embedBuilder.Color = Color.DarkRed;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Formats the message by wrapping it in code block ticks and truncating if it exceeds the specified maximum length.
        /// </summary>
        /// <param name="message">The message to format.</param>
        /// <param name="maxLenght">The maximum allowed length for the message.</param>
        /// <returns>The formatted message.</returns>
        public static string FormatMessage(string message, int maxLenght)
        {
            return message.Length > maxLenght
                ? $"```{message.Substring(0, maxLenght)} ...```"
                : $"```{message}```";
        }
        /// <summary>
        /// Determines whether a message should be logged based on its log level and the minimum log level specified.
        /// </summary>
        /// <param name="minimumLogEventLevel">The minimum log event level required to log a message.</param>
        /// <param name="messageLogEventLevel">The log event level of the message in question.</param>
        /// <returns>True if the message should be logged, false otherwise.</returns>
        private static bool ShouldLogMessage(LogEventLevel minimumLogEventLevel, LogEventLevel messageLogEventLevel) =>
            (int)messageLogEventLevel >= (int)minimumLogEventLevel;
    }
}
