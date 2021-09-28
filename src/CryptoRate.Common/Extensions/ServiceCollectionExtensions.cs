﻿using CryptoRate.Common.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable UnusedMethodReturnValue.Global

namespace CryptoRate.Common.Extensions {

	public static class ServiceCollectionExtensions {

		public static IServiceCollection AddOptions<TOptions>(this IServiceCollection services, IConfiguration configuration, string sectionName) where TOptions : class {
			services.Configure<TOptions>(configuration.GetSection(sectionName));
			services.AddOptionsValidator<TOptions>();
			return services;
		}

		public static IServiceCollection AddOptionsValidator<TOptions>(this IServiceCollection services) where TOptions : class {
			services.AddSingleton<IValidateOptions<TOptions>, OptionsValidator<TOptions>>();
			return services;
		}

	}

}