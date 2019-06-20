## [Serilog](https://serilog.net) [discord](https://discordapp.com) sink.

### what is it? 🤔

A sink for serilog that let you write your logs to discord.

### how to use it ? 🤔

To use discord sink you need **WebhookId** and **WebhookToken**

to get them, follow this steps :

0. Login to discord
1. Create discord Server
2. Create text chanel for logs
3. In chanel setting section, Create webhook


After createing webhook, you get a webhook url that contains **WebhookId** and **WebhookToken**.
`https://discordapp.com/api/webhooks/[WebhookId]/[WebhookToken]`

now every thing is ready.                                                                            
`Log.Logger = new LoggerConfiguration().WriteTo.Discord(ulong.Parse([WebhookId]), [WebhookToken]).CreateLogger();`


### Any thing to see? 🤔

![Serilog](/Screenshots/logs1.png?raw=true)

![Serilog](/Screenshots/logs.png?raw=true)
