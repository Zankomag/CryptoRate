using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using CryptoRate.Bot.Services;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Bot {

	public class Startup {

		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration) => Configuration = configuration;

		public void ConfigureServices(IServiceCollection services) {
			services.AddOptions<TelegramBotOptions>(Configuration, TelegramBotOptions.SectionName);
			services.AddScoped<ITelegramBotService, TelegramBotService>();
		}

	}

}