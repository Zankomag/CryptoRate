using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Core.Abstractions {

	public interface IStartup {

		IConfiguration Configuration { get; }

		void ConfigureServices(IServiceCollection services);

	}

}