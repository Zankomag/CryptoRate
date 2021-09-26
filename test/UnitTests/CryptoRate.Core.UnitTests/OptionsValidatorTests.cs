using CryptoRate.Core.Configs;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CryptoRate.Core.UnitTests {

	public class OptionsValidatorTests {

		private static readonly ServiceCollection services = new();

		private static void ConfigureCryptoClientOptions(string apiKey) => services.Configure<CryptoClientOptions>(o => o.ApiKey = apiKey);

		[Fact]
		public void Validate_Succeeds_When_CryptoClient_Config_IsRight() {

			//Arrange
			ConfigureCryptoClientOptions("MyRightApiKey");
			services.AddOptionsValidator<CryptoClientOptions>();
			
			var serviceProvider = services.BuildServiceProvider();

			//Act
			var options = serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>();

			//Assert
			Assert.NotNull(options.Value);

		}

		[Theory]
		[InlineData((string)null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("    ")]
		[InlineData(" ")] //alt + 255
		[InlineData("\t")]
		[InlineData("\n")]
		[InlineData(" MyRightApiKey")]
		public void Validate_Throws_When_CryptoClient_Config_IsNullOrWhiteSpace(string apiKey) {

			//Arrange
			ConfigureCryptoClientOptions(apiKey);
			services.AddOptionsValidator<CryptoClientOptions>();

			var serviceProvider = services.BuildServiceProvider();

			//Act + Assert
			Assert.Throws<OptionsValidationException>(() =>
				serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>().Value);
		}

	}

}