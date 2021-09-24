using System;
using CryptoRate.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Core {

	public static class Startup {

		private static IConfigurationRoot configuration;

		static Startup() {
			Initialize();
			var serviceCollection = new ServiceCollection();
			ConfigureBasicServices(serviceCollection);
			ConfigureConsumerServices(serviceCollection);
			ConfigureStepFunctionInvoker(serviceCollection);
		}

		public static void Initialize() {
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false)
				.AddJsonFile($"appsettings.{EnvironmentUtils.GetEnvironmentName()}.json", optional: true);
			configuration = builder.Build();
		}

	}

}