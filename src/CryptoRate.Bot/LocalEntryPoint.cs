using System.Threading.Tasks;
using CryptoRate.Bot.Services;
using CryptoRate.Common.Extensions;
using CryptoRate.Common.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoRate.Bot {

	public class LocalEntryPoint {

		private static async Task Main() {
			IHost host;
			if(EnvironmentWrapper.IsDevelopment) {
				host = GetHost();
			} else {
				host = GetWebHost();
			}

			await host.RunAsync();
		}

		private static IHost GetHost()
			=> new HostBuilder()
				.AddConfiguration()
				.UseStartup<Startup>()
				.ConfigureServices(services => services.AddHostedService<TelegramBotLocalRunner>())
				.Build();

		private static IHost GetWebHost()
			=> new HostBuilder()
				.AddConfiguration()
				.ConfigureWebHost(x =>
					x.UseKestrel((builderContext, options) => options.Configure(builderContext.Configuration.GetSection("Kestrel")))
						.UseStartup<Startup>())
				.Build();

	}

}