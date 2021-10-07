using System.IO;
using System.Text;
using CryptoRate.Core.Configs;
using CryptoRate.Common.Extensions;
using CryptoRate.Common.UnitTests.Fixtures;
using CryptoRate.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CryptoRate.Common.UnitTests {

	public class ServiceCollectionExtensionsTests {

		private static readonly ServiceCollection services = new ServiceCollection();

		private static IConfiguration GetMockedCryptoClientConfiguration(string apiKey) {
			var apiKeyConfigurationSectionMock = new Mock<IConfigurationSection>();
			apiKeyConfigurationSectionMock.Setup(c => c.Value).Returns(apiKey);
			var cryptoClientOptionsConfigurationSectionMock = new Mock<IConfigurationSection>();
			cryptoClientOptionsConfigurationSectionMock.Setup(x => x.GetSection(nameof(CryptoClientOptions.ApiKey))).Returns(apiKeyConfigurationSectionMock.Object);
			cryptoClientOptionsConfigurationSectionMock.Setup(x => x.GetChildren()).Returns(new [] {apiKeyConfigurationSectionMock.Object});
			var configurationMock = new Mock<IConfiguration>();
			configurationMock.Setup(x => x.GetSection(CryptoClientOptions.SectionName)).Returns(cryptoClientOptionsConfigurationSectionMock.Object);
			return configurationMock.Object;
		}

		//This method is the same as GetMockedCryptoClientConfiguration, but does not use mocks
		private static IConfiguration GetCryptoClientConfiguration(string apiKey) {
			var appSettings = 
				@$"{{
                       ""{CryptoClientOptions.SectionName}"": {{
                           ""{nameof(CryptoClientOptions.ApiKey)}"": ""{apiKey}""
                       }}
                   }}";

			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

			return configurationBuilder.Build();
		}

		[Theory]
		[MemberData(nameof(CryptoClientFixture.RightApiKeys), MemberType = typeof(CryptoClientFixture))]
		public void AddOptionsValidator_Succeeds_When_CryptoClient_Config_IsRight(string apiKey) {

			//Arrange
			var configuration = GetMockedCryptoClientConfiguration(apiKey);
			services.AddOptions<CryptoClientOptions>(configuration, CryptoClientOptions.SectionName);
			
			//Act
			var serviceProvider = services.BuildServiceProvider();
			var options = serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>();

			//Assert
			Assert.NotNull(options.Value);

		}

		[Theory]
		[MemberData(nameof(CryptoClientFixture.WrongApiKeys), MemberType = typeof(CryptoClientFixture))]
		public void AddOptionsValidator_Throws_When_CryptoClient_Config_IsNullOrWhiteSpace(string apiKey) {

			//Arrange
			var configuration = GetMockedCryptoClientConfiguration(apiKey);
			services.AddOptions<CryptoClientOptions>(configuration, CryptoClientOptions.SectionName);
			
			//Act + Assert
			var serviceProvider = services.BuildServiceProvider();
			Assert.Throws<OptionsValidationException>(() =>
				serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>().Value);
		}

		[Theory]
		[MemberData(nameof(CryptoClientFixture.RightApiKeys), MemberType = typeof(CryptoClientFixture))]
		public void ConfigureServices_Succeeds_When_CryptoClient_Config_IsRight(string apiKey) {

			//Arrange
			var configuration = GetCryptoClientConfiguration(apiKey);
			var startup = new Startup(configuration);

			//Act
			startup.ConfigureServices(services);
			var serviceProvider = services.BuildServiceProvider();
			var options = serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>();

			//Assert
			Assert.NotNull(options.Value);

		}

		[Theory]
		[MemberData(nameof(CryptoClientFixture.WrongApiKeysForJson), MemberType = typeof(CryptoClientFixture))]
		public void ConfigureServices_Throws_When_CryptoClient_Config_IsNullOrWhiteSpace(string apiKey) {

			//Arrange
			var configuration = GetCryptoClientConfiguration(apiKey);
			var startup = new Startup(configuration);

			//Act + Assert
			startup.ConfigureServices(services);
			var serviceProvider = services.BuildServiceProvider();
			Assert.Throws<OptionsValidationException>(() =>
				serviceProvider.GetRequiredService<IOptions<CryptoClientOptions>>().Value);
		}
	}

}