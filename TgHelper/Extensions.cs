using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace TgHelper;

public static class Extensions
{
    public static IServiceCollection AddTelegramBotClientWithWebhooks(this IServiceCollection services)
    {
        services.AddTransient<ITelegramBotClient>(sp =>
        {
            return new TelegramBotClient(sp.GetService<IConfiguration>()!["TgBot:Token"]!);
        });

        services.AddHostedService<ConfigureWebhook>();

        return services;
    }
}