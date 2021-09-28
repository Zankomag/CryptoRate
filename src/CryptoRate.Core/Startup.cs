using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Core {

	public class Startup : StartupBase {

		public Startup(IConfiguration configuration) : base(configuration) { }

		public override void ConfigureServices(IServiceCollection services) => services.AddCryptoClientAsScoped(Configuration);

	}

}