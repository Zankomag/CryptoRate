using CryptoRate.Core.Configs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CryptoRate.Core.UnitTests {

	public class OptionValidatorTests {

		[Fact]
		public void Validate_Succeeds_When_CryptoClient_Config_IsRight() {

			//Arrange
			var services = new ServiceCollection();
			services.AddOptions<CryptoClientOptions>()
				.Configure(o => o.ApiKey = "MyRightApiKey")
				.ValidateDataAnnotations();
			var serviceProvider = services.BuildServiceProvider();

			//Act
			var exception = Record.Exception(() => serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>());

			//Assert
			Assert.Null(exception);
		}

		[Theory]
		[InlineData((string)null)]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("    ")]
		[InlineData(" ")] //alt + 255
		[InlineData("\t")]
		[InlineData("\n")]
		public void Validate_Throws_When_CryptoClient_Config_IsNullOrWhiteSpace(string apiKey) {

			//Arrange
			var services = new ServiceCollection();
			services.AddOptions<CryptoClientOptions>()
				.Configure(o => o.ApiKey = apiKey)
				.ValidateDataAnnotations();
			var serviceProvider = services.BuildServiceProvider();

			//Act + Assert
			Assert.Throws<OptionsValidationException>(() =>
				serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>().Value);
		}

	}

}