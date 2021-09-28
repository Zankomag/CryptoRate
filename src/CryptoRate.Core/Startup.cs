using CryptoRate.Common.Abstractions;
using CryptoRate.Common.Extensions;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Configs;
using CryptoRate.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Core {

	public class Startup : StartupBase {

		public Startup(IConfiguration configuration) : base(configuration) { }

		public override void ConfigureServices(IServiceCollection services) {
			services.AddOptions<CryptoClientOptions>(Configuration, CryptoClientOptions.SectionName);
			services.AddScoped<ICryptoClient, CryptoClient>();
		}

	}

}