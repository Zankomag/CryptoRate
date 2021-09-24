using System;
using System.Threading.Tasks;
using CryptoRate.Core;
using static System.Console;

namespace CtyptoRate.Console {

	public class Program {

		private static async Task Main(string[] args) {
			CryptoRateService cryptoRateService = new("");
			var currencyRate = await cryptoRateService.GetCurrencyRate("BTC", "USD");
			WriteLine(Decimal.Round(currencyRate.rate, MidpointRounding.ToZero));
			ReadLine();
		}

	}

}