using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using CryptoRate.Bot.Services;
using CryptoRate.Common.Abstractions;
using CryptoRate.Common.Extensions;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Bot {

	public class Startup : StartupBase {

		public Startup(IConfiguration configuration) : base(configuration) { }

		public override void ConfigureServices(IServiceCollection services) {
			//TODO Move to the extension methods
			services.AddCryptoRateServices(Configuration);
			services.AddOptions<TelegramBotOptions>(Configuration, TelegramBotOptions.SectionName);
			services.AddSingleton<ITelegramBotService, TelegramBotService>();
		}

	}

}