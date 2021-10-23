using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using CryptoRate.Bot.Services;
using CryptoRate.Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Bot.Extensions {

	public static class ServiceCollectionExtensions {

		public static IServiceCollection AddTelegramBotServices(this IServiceCollection services, IConfiguration configuration) {
			services.AddOptions<TelegramBotOptions>(configuration, TelegramBotOptions.SectionName);
			services.AddSingleton<ITelegramBotService, TelegramBotService>();
			return services;
		}

	}

}