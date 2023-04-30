using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace TgHelper
{
    public class ConfigureWebhook : IHostedService
    {
        private readonly ILogger<ConfigureWebhook> _logger;

        private readonly IConfiguration _config;

        private readonly ITelegramBotClient _botClient;

        public ConfigureWebhook(IConfiguration config, ILogger<ConfigureWebhook> logger,
            ITelegramBotClient botClient)
        {
            _config = config;
            _logger = logger;
            _botClient = botClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"StartAsync: Start");

            for (int i = 1; i <= 10; i++)
            {
                try
                {
                    var url = $"{_config["TgBot:Address"]}/{_config["TgBot:Route"]}";
                    _logger.LogInformation($"StartAsync: Попытка настроить вебхук: {i}, url: {url}");
                    await _botClient.SetWebhookAsync(
                        url: url,
                        //allowedUpdates: Array.Empty<UpdateType>(),
                        cancellationToken: cancellationToken);

                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "StartAsync");

                    if (i == 10)
                    {
                        throw;
                    }

                    await Task.Delay(1000 * i, cancellationToken);
                }
            }

            _logger.LogInformation($"StartAsync: End");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"StopAsync");

            await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}