using System;
using System.Threading.Tasks;
using CryptoRate.Console.Extensions;
using CryptoRate.Core;
using CryptoRate.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Console;

namespace CryptoRate.Console {

	public class Program {

		private static async Task Main(string[] args) {
			var host = new HostBuilder()
				.AddConfiguration()
				.UseStartup<Startup>()
				.Build();

			//await host.RunAsync();
			var cryptoClient = host.Services.GetRequiredService<ICryptoClient>();
			var currencyRate = await cryptoClient.GetCurrencyRate("BTC", "USD");
			WriteLine($"1 BTC = {Decimal.Round(currencyRate.rate, MidpointRounding.ToZero)} USD");
		}

	}

}