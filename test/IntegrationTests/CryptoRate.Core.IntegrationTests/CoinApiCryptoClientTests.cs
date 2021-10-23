using System;
using System.Threading.Tasks;
using CryptoRate.Common.Extensions;
using CryptoRate.Common.Utils;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Enums;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace CryptoRate.Core.IntegrationTests {

	public class CoinApiCryptoClientTests {

		private readonly IHost host;

		public CoinApiCryptoClientTests() {
			//TODO try the other way to set env
			Environment.SetEnvironmentVariable(EnvironmentWrapper.EnvironmentName, EnvironmentWrapper.Development);

			host = new HostBuilder()
				.AddConfiguration()
				.ConfigureServices((context, services) => services.AddCryptoRateServices(context.Configuration))
				.Build();
		}

		public ICryptoClient GetCryptoClient() => host.Services.GetRequiredService<ICryptoClient>();

		[Fact]
		public void GetRequiredService_Succeeds_WhenGettingCryptoClient() {

			//Act
			var cryptoClient = host.Services.GetRequiredService<ICryptoClient>();

			//Assert
			Assert.NotNull(cryptoClient);
		}

		[Fact]
		public async Task GetCurrencyRate_Succeeds_WhenGettingBtcToUsd() {

			//Arrange
			var cryptoClient = GetCryptoClient();
			const string baseCurrency = CurrencyCode.Bitcoin;
			const string quoteCurrency = CurrencyCode.Usd;

			//Act
			var result = await cryptoClient.GetCurrencyRate(baseCurrency, quoteCurrency);

			//Assert
			Assert.NotNull(result);
			Assert.NotEqual(default, result.rate);
			Assert.Equal(baseCurrency, result.asset_id_base);
			Assert.Equal(quoteCurrency, result.asset_id_quote);
		}

	}


}