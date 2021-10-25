using System;
using System.Threading.Tasks;
using CoinAPI.REST.V1;
using CoinAPI.REST.V1.Exceptions;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoRate.Core.Services {

	public class CoinApiCryptoClient : ICryptoClient {

		private readonly ILogger<CoinApiCryptoClient> logger;

		private readonly CoinApiRestClient client;

		public CoinApiCryptoClient(IOptions<CryptoClientOptions> options, ILogger<CoinApiCryptoClient> logger) {
			this.logger = logger;
			client = new CoinApiRestClient(options.Value.ApiKey);
		}

		//TODO Add handler if currency code in request was wrong
		public async Task<Exchangerate> GetCurrencyRateAsync(string currencyBase, string currencyQuote) {
			if(String.IsNullOrWhiteSpace(currencyBase)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(currencyBase));
			if(String.IsNullOrWhiteSpace(currencyQuote)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(currencyQuote));
			Exchangerate result = null;
			try {
				result = await client.Exchange_rates_get_specific_rateAsync(currencyBase, currencyQuote);
			} catch(CoinApiException ex) {
				logger.LogError(ex, "CoinAPI exception: ");
			}
			return result;
		}

	}

}