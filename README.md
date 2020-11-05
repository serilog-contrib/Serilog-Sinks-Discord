# Serilog sink for Discord

### Write your logs to discord.

### To get started:
#### Step :one: : get **WebhookId** and **WebhookToken**.

follow these steps to get them: \
  a. login to discord \
  b. create a discord Server \
  c. create a text chanel for your logs \
  d. create new webhook from chanel setting \
  e. copy webhook url

the link contains **WebhookId** and **WebhookToken** \
`https://discordapp.com/api/webhooks/[WebhookId]/[WebhookToken]`

#### Step :two: : install [nuget package](https://www.nuget.org/packages/Serilog.Sinks.Discord/) on your project

#### Step :three: : configure logger to write to discord:

 `Log.Logger =` \
  `new LoggerConfiguration()` \
  `.WriteTo.Discord(ulong.Parse([WebhookId]), [WebhookToken])` \
  `.CreateLogger();`
\
\
\
![Serilog](/Screenshots/logs1.png?raw=true)

![Serilog](/Screenshots/logs.png?raw=true)
