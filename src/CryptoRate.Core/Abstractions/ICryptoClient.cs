using System.Threading.Tasks;
using CoinAPI.REST.V1;

namespace CryptoRate.Core.Abstractions {

	public interface ICryptoClient {

		//TODO add new format that contains char fields too and map to it via CurrencyCode extensions and mapper
		Task<Exchangerate> GetCurrencyRate(string currencyBase, string currencyQuote);

	}

}