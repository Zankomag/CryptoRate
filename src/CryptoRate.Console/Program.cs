using System;
using System.Threading.Tasks;
using CryptoRate.Common.Extensions;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Enums;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace CryptoRate.Console {

	public class Program {

		private static async Task Main() {
			var host = new HostBuilder()
				.AddConfiguration()
				.ConfigureServices((context, services) => {
					services.AddLogging(logging => logging.AddConsole());
					services.AddCryptoRateServices(context.Configuration);
				})
				.Build();

			var logger = host.Services.GetRequiredService<ILogger<Program>>();
			logger.LogInformation("Requesting BTC to USD exchange rate");
			var cryptoClient = host.Services.GetRequiredService<ICryptoClient>();
			var currencyRate = await cryptoClient.GetCurrencyRate(CurrencyCode.Bitcoin, CurrencyCode.Usd);
			logger.LogInformation($"1 BTC = {Decimal.Round(currencyRate.rate, MidpointRounding.ToZero)} USD");
		}

	}

}