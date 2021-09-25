using System;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Configs;
using CryptoRate.Core.Extensions;
using CryptoRate.Core.Services;
using CryptoRate.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Core {

	public static class Startup {

		public static IServiceProvider ServiceProvider { get; set; }
		
		private static IConfigurationRoot configuration;

		static Startup() {
			Initialize();
			var services = new ServiceCollection();
			ConfigureBasicServices(services);
		}

		private static void ConfigureBasicServices(IServiceCollection services) {
			//services.Configure<CryptoClientOptions>(configuration.GetSection("CryptoClient"));
			services.AddCryptoClientAsSingleton(configuration);
		}

		public static void Initialize() {
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.AddJsonFile($"appsettings.{EnvironmentWrapper.GetEnvironmentName()}.json", true);
			configuration = builder.Build();
		}

	}

}