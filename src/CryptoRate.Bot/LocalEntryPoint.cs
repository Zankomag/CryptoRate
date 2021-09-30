using System.Threading.Tasks;
using CryptoRate.Bot.Services;
using CryptoRate.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoRate.Bot {

	public class LocalEntryPoint {

		private static async Task Main() {
			var host = new HostBuilder()
				.AddConfiguration()
				.UseStartup<Core.Startup>()
				.UseStartup<Startup>()
				.ConfigureServices(services => services.AddHostedService<TelegramBotLocalRunner>())
				.Build();

			await host.RunAsync();
		}

	}

}