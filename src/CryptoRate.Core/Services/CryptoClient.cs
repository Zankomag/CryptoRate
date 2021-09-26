using System;
using System.Threading.Tasks;
using CoinAPI.REST.V1;
using CoinAPI.REST.V1.Exceptions;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Configs;
using Microsoft.Extensions.Options;

namespace CryptoRate.Core.Services {

	public class CryptoClient : ICryptoClient {

		private readonly CoinApiRestClient client;

		public CryptoClient(IOptions<CryptoClientOptions> options) {
			client = new CoinApiRestClient(options.Value.ApiKey);
		}

		public async Task<Exchangerate> GetCurrencyRate(string currencyBase, string currencyQuote) {
			if(String.IsNullOrWhiteSpace(currencyBase)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(currencyBase));
			if(String.IsNullOrWhiteSpace(currencyQuote)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(currencyQuote));
			Exchangerate result = null;
			try {
				result = await client.Exchange_rates_get_specific_rateAsync(currencyBase, currencyQuote);
			} catch(CoinApiException ex) {
				//TODO Log exception
			}
			return result;
		}

	}

}