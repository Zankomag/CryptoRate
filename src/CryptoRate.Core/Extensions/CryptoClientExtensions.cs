using System.Threading.Tasks;
using CoinAPI.REST.V1;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Enums;

namespace CryptoRate.Core.Extensions {

	public static class CryptoClientExtensions {

		public static async Task<Exchangerate> GetBitcoinToUsdCurrencyRate(this ICryptoClient cryptoClient) => await cryptoClient.GetCurrencyRate(Currency.Bitcoin, Currency.Usd);

		public static async Task<Exchangerate> GetEthereumToUsdCurrencyRate(this ICryptoClient cryptoClient) => await cryptoClient.GetCurrencyRate(Currency.Ethereum, Currency.Usd);

	}

}