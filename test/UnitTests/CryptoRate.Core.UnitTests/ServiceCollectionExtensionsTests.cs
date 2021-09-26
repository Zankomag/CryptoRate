using System.IO;
using System.Text;
using CryptoRate.Core.Configs;
using CryptoRate.Core.Extensions;
using CryptoRate.Core.UnitTests.Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CryptoRate.Core.UnitTests {

	public class ServiceCollectionExtensionsTests {

		private static readonly ServiceCollection services = new();

		private static void ConfigureCryptoClientOptions(string apiKey) => services.Configure<CryptoClientOptions>(o => o.ApiKey = apiKey);

		private static IConfiguration GetCryptoClientConfiguration(string apiKey) {
			var appSettings = @$"{{""CryptoClient"": {{""ApiKey"": ""{apiKey}""}}}}";

			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

			return configurationBuilder.Build();
		}

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

		[Theory]
		[MemberData(nameof(CryptoClientFixture.RightApiKeys), MemberType = typeof(CryptoClientFixture))]
		public void AddCryptoClientAsSingleton_Succeeds_When_CryptoClient_Config_IsRight(string apiKey) {

			//Arrange
			var configuration = GetCryptoClientConfiguration(apiKey);

			//Act
			services.AddCryptoClientAsSingleton(configuration);
			var serviceProvider = services.BuildServiceProvider();
			var options = serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>();

			//Assert
			Assert.NotNull(options.Value);

		}

		[Theory]
		[MemberData(nameof(CryptoClientFixture.WrongApiKeysForJson), MemberType = typeof(CryptoClientFixture))]
		public void AddCryptoClientAsSingleton_Throws_When_CryptoClient_Config_IsNullOrWhiteSpace(string apiKey) {

			//Arrange
			var configuration = GetCryptoClientConfiguration(apiKey);

			//Act + Assert
			services.AddCryptoClientAsSingleton(configuration);
			var serviceProvider = services.BuildServiceProvider();
			Assert.Throws<OptionsValidationException>(() =>
				serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>().Value);
		}
	}

}