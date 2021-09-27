using System.Threading.Tasks;
using CryptoRate.Bot.Services;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoRate.Bot {

	public class LocalEntryPoint {

		private static async Task Main(string[] args) {
			var host = new HostBuilder()
				.AddConfiguration()
				.UseStartup<Core.Startup>()
				.UseStartup<Startup>()
				.ConfigureServices(services => services.AddHostedService<TelegramBotLocalHostedService>())
				.Build();

			await host.RunAsync();
		}

	}

}