using CryptoRate.Core.Configs;
using CryptoRate.Core.Extensions;
using CryptoRate.Core.UnitTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CryptoRate.Core.UnitTests {

	public class OptionsValidatorTests {

		private static readonly ServiceCollection services = new();

		private static void ConfigureCryptoClientOptions(string apiKey) => services.Configure<CryptoClientOptions>(o => o.ApiKey = apiKey);

		[Theory]
		[MemberData(nameof(CryptoClientFixture.RightApiKeys), MemberType = typeof(CryptoClientFixture))]
		public void AddOptionsValidator_Succeeds_When_CryptoClient_Config_IsRight(string apiKey) {

			//Arrange
			ConfigureCryptoClientOptions(apiKey);
			
			//Act
			services.AddOptionsValidator<CryptoClientOptions>();
			var serviceProvider = services.BuildServiceProvider();
			var options = serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>();

			//Assert
			Assert.NotNull(options.Value);

		}

		[Theory]
		[MemberData(nameof(CryptoClientFixture.WrongApiKeys), MemberType = typeof(CryptoClientFixture))]
		public void AddOptionsValidator_Throws_When_CryptoClient_Config_IsNullOrWhiteSpace(string apiKey) {

			//Arrange
			ConfigureCryptoClientOptions(apiKey);
			
			//Act + Assert
			services.AddOptionsValidator<CryptoClientOptions>();
			var serviceProvider = services.BuildServiceProvider();
			Assert.Throws<OptionsValidationException>(() =>
				serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>().Value);
		}

	}

}