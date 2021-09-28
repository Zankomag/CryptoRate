using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Core.Abstractions {

	public abstract class StartupBase : IStartup {

		public IConfiguration Configuration { get; }

		protected StartupBase(IConfiguration configuration) => Configuration = configuration;

		/// <inheritdoc />
		public abstract void ConfigureServices(IServiceCollection services);

	}

}