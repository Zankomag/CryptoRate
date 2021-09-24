using System;
using System.Threading.Tasks;
using CoinAPI.REST.V1;

namespace CryptoRate.Core {

	public class CryptoRateService {

		private readonly CoinApiRestClient client;

		public CryptoRateService(string apiKey) {
			if(String.IsNullOrWhiteSpace(apiKey)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(apiKey));
			client = new CoinApiRestClient(apiKey);
		}

		public async Task<Exchangerate> GetCurrencyRate(string currencyBase, string currencyQuote) {
			if(String.IsNullOrWhiteSpace(currencyBase)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(currencyBase));
			if(String.IsNullOrWhiteSpace(currencyQuote)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(currencyQuote));
			Exchangerate result = await client.Exchange_rates_get_specific_rateAsync(currencyBase, currencyQuote);
			return result;
		}
		
		

	}

}