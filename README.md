# Serilog sink for Discord

### Write your logs to discord.

### To get started:
#### Step :one: : get **WebhookId** and **WebhookToken**.

webhook url contains **WebhookId** and **WebhookToken** \
`https://discordapp.com/api/webhooks/[WebhookId]/[WebhookToken]`

[how to create webhook url](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks)

#### Step :two: : install [nuget package](https://www.nuget.org/packages/Serilog.Sinks.Discord/) on your project

#### Step :three: : configure logger writes to discord:
 `Log.Logger =` \
  `new LoggerConfiguration()` \
  `.WriteTo.Discord(ulong.Parse([WebhookId]), [WebhookToken])` \
  `.CreateLogger();`

or with MinimumLevel:

`Log.Logger =` \
`new LoggerConfiguration()` \
`.WriteTo.Discord(ulong.Parse([WebhookId]), [WebhookToken], [restrictedToMinimumLevel])` \
`.CreateLogger();`

### Configuration by appsettings.json

```json
{
  "Serilog": {
    "WriteTo": [
    {
      "Name": "Discord",
      "Args": {
        "webhookId" : [discordWebHookId],
        "webhookToken" : [discordWebhookToken],
        "restrictedToMinimumLevel" : [minimumLevelForDiscordSink]
      }
    }]
  }
}
```

### Screenshots:

![Serilog](/Screenshots/logs.png?raw=true)

![Serilog](/Screenshots/logs1.png?raw=true)