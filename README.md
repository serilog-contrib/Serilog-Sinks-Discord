# Serilog sink for Discord

### Write your logs to discord.

![Serilog](/Screenshots/screenshot.png?raw=true)

![Serilog](/Screenshots/screenshot1.png?raw=true)

 #### To get started:
 :one:: Get ```WebhookId``` and ```WebhookToken``` </br>
> [Create webhoook](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks) and copy its url
which contains WebhookId and WebhookToken: </br>
```https://discordapp.com/api/webhooks/{WebhookId}/{WebhookToken}```

:two:: Install [nuget package](https://www.nuget.org/packages/Serilog.Sinks.Discord/)

:three:: Add discord output: </br>
 ```csharp
Log.Logger = new LoggerConfiguration()
  .WriteTo.Discord({WebhookId}, {WebhookToken})
  .CreateLogger();
```
for async logging you can use [serilog-sinks-async](https://github.com/serilog/serilog-sinks-async)
```C#
Log.Logger = new LoggerConfiguration()
 .WriteTo.Async( a => 
     a.Discord({WebhookId}, {WebhookToken}))
 .Enrich.FromLogContext()
 .CreateLogger();
```
