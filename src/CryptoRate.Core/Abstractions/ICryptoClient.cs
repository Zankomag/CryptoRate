using System.Threading.Tasks;
using CoinAPI.REST.V1;

namespace CryptoRate.Core.Abstractions {

	public interface ICryptoClient {

		Task<Exchangerate> GetCurrencyRate(string currencyBase, string currencyQuote);

	}

}