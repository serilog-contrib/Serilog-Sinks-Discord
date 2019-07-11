## [Serilog](https://serilog.net) [discord](https://discordapp.com) sink.

### What is it? 🤔

A sink for serilog that let you write your logs to discord.

### How to use it ? 🤔

First install package from
https://www.nuget.org/packages/Serilog.Sinks.Discord/

then you need **WebhookId** and **WebhookToken**

to get them, follow these steps :

0. login to discord
1. create a discord Server
2. create a text chanel for logs
3. in chanel setting section, Create webhook

ffter createing webhook, you get a webhook url which contains **WebhookId** and **WebhookToken**.
`https://discordapp.com/api/webhooks/[WebhookId]/[WebhookToken]`

now every thing is ready.                                                                            
`Log.Logger = new LoggerConfiguration().WriteTo.Discord(ulong.Parse([WebhookId]), [WebhookToken]).CreateLogger();`


### Any thing to see? 🤔

![Serilog](/Screenshots/logs1.png?raw=true)

![Serilog](/Screenshots/logs.png?raw=true)
