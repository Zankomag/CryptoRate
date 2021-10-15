using System;
using System.Threading.Tasks;
using CryptoRate.Common.Extensions;
using CryptoRate.Core;
using CryptoRate.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace CryptoRate.Console {

	public class Program {

		private static async Task Main() {
			var host = new HostBuilder()
				.AddConfiguration()
				.UseStartup<Startup>()
				.ConfigureServices(x => x.AddLogging(logging => logging.AddConsole()))
				.Build();

			var logger = host.Services.GetRequiredService<ILogger<Program>>();
			logger.LogInformation("Requesting BTC to USD exchange rate");
			var cryptoClient = host.Services.GetRequiredService<ICryptoClient>();
			var currencyRate = await cryptoClient.GetBitcoinToUsdCurrencyRate();
			logger.LogInformation($"1 BTC = {Decimal.Round(currencyRate.rate, MidpointRounding.ToZero)} USD");
		}

	}

}