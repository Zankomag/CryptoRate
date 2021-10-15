using CryptoRate.Common.Extensions;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Configs;
using CryptoRate.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Core.Extensions {

	public static class ServiceCollectionExtensions {

		public static IServiceCollection AddCryptoRateServices(this IServiceCollection services, IConfiguration configuration) {
			services.AddOptions<CryptoClientOptions>(configuration, CryptoClientOptions.SectionName);
			services.AddScoped<ICryptoClient, CoinApiCryptoClient>();
			return services;
		}

	}

}