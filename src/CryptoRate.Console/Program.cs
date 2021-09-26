using System;
using System.Threading.Tasks;
using CryptoRate.Core;
using CryptoRate.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using static System.Console;

namespace CryptoRate.Console {

	public class Program {

		private static async Task Main(string[] args) {
			var cryptoClient = Startup.ServiceProvider.GetRequiredService<ICryptoClient>();
			var currencyRate = await cryptoClient.GetCurrencyRate("BTC", "USD");
			WriteLine($"1 BTC = {Decimal.Round(currencyRate.rate, MidpointRounding.ToZero)} USD");
			ReadLine();
		}

	}

}