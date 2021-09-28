using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using CryptoRate.Bot.Services;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Bot {

	public class Startup : StartupBase {

		public Startup(IConfiguration configuration) : base(configuration) { }

		public override void ConfigureServices(IServiceCollection services) {
			services.AddOptions<TelegramBotOptions>(Configuration, TelegramBotOptions.SectionName);
			services.AddScoped<ITelegramBotService, TelegramBotService>();
		}

	}

}