namespace CryptoRate.Core.Enums {

	/// <summary>
	///     Represents constants with currency codes and chars
	/// </summary>
	public static class CurrencyCode {

		public const string Bitcoin = "BTC";
		public const string BitcoinChar = "₿";
		
		public const string Ethereum = "ETH";
		public const string EthereumChar = "Ξ";

		/// <summary>
		///     United States Dollar
		/// </summary>
		public const string Usd = "USD";
		public const string UsdChar = "$";

		public static string GetCurrencyCharByCode(this string currencyCode)
			=> currencyCode switch {
				Bitcoin => BitcoinChar,
				Ethereum => EthereumChar,
				Usd => UsdChar,
				_ => currencyCode
			};

	}

}