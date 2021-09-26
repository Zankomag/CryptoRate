using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoRate.Bot.Services;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Extensions.Polling;

namespace CryptoRate.Bot {

	public class LocalEntryPoint {

		private static async Task Main(string[] args) {
			var host = new HostBuilder()
				.AddConfiguration()
				.UseStartup<Core.Startup>()
				.UseStartup<Startup>()
				.ConfigureServices(services => services.AddSingleton/*AddHostedService*/<IUpdateHandler, TelegramBotLocalHostedService>())
				.Build();
//
			var service = host.Services.GetRequiredService<IUpdateHandler>() as TelegramBotLocalHostedService;
			await service.StartAsync(new CancellationTokenSource().Token);
			Console.ReadLine();

			//await host.RunAsync();
		}

	}

}