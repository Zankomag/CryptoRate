using CryptoRate.Bot.Services;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoRate.Bot {

	public class LocalEntryPoint {

		private static void Main(string[] args) {
			var host = new HostBuilder()
				.AddConfiguration()
				.UseStartup<Core.Startup>()
				.UseStartup<Startup>()
				.ConfigureServices(services => services.AddHostedService<TelegramBotLocalHostedService>())
				.Build();

			host.Run();
		}

	}

}