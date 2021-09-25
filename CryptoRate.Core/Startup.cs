using CryptoRate.Core.Configs;
using CryptoRate.Core.Extensions;
using CryptoRate.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Core {

	public static class Startup {

		private static IConfigurationRoot configuration;

		static Startup() {
			Initialize();
			var services = new ServiceCollection();
			ConfigureBasicServices(services);
		}

		private static void ConfigureBasicServices(IServiceCollection services) {
			services.Configure<CryptoClientOptions>(configuration.GetSection("ExceptionMiddlewareOptions"));
		}

		public static void Initialize() {
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", false)
				.AddJsonFile($"appsettings.{EnvironmentWrapper.GetEnvironmentName()}.json", true);
			configuration = builder.Build();
		}

	}

}