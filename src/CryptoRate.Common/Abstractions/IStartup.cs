using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Common.Abstractions {

	public interface IStartup {

		IConfiguration Configuration { get; }

		void ConfigureServices(IServiceCollection services);

	}

}