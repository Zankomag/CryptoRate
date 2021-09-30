using Microsoft.Extensions.DependencyInjection;

namespace CryptoRate.Common.Abstractions {

	public interface IStartup {

		void ConfigureServices(IServiceCollection services);

	}

}