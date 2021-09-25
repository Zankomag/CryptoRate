using System;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Configs;
using CryptoRate.Core.Services;
using CryptoRate.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CryptoRate.Core.Extensions {

	public static class ServiceCollectionExtensions {

		public static IServiceCollection AddCryptoClientAsSingleton(this IServiceCollection services, IConfigurationRoot configuration) {
			services.AddOptions<CryptoClientOptions>().ValidateDataAnnotations(); //.ValidateOptions();//
			services.AddSingleton<ICryptoClient, CryptoClient>();
			return services;
		}

		public static OptionsBuilder<TOptions> ValidateOptions<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class {
			optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>, OptionsValidator<TOptions>>();
			return optionsBuilder;
		}

	}

}