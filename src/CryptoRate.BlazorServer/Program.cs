using System;
using System.IO;
using System.Threading.Tasks;
using CryptoRate.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.Hosting;

namespace CryptoRate.BlazorServer {

	public class Program {

		public static async Task Main(string[] args) =>

			//await CreateHostBuilder(args).Build().RunAsync();
			await GetWebHost().RunAsync();

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

		private static IHost GetWebHost()
			=> new HostBuilder()
				.AddConfiguration()
				.ConfigureWebHost(webHostBuilder => {
					webHostBuilder.UseKestrel();

					//Static .css files doesn't work without this. For more info look here: https://github.com/dotnet/aspnetcore/issues/28911, https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files, https://dev.to/j_sakamoto/how-to-deal-with-the-http-404-content-foo-bar-css-not-found-when-using-razor-component-package-on-asp-net-core-blazor-app-aai
					webHostBuilder.UseStaticWebAssets();
					webHostBuilder.UseContentRoot(Directory.GetCurrentDirectory()); //wwwroot files don't load without this line

					//x.UseUrls("https://*:5930");
					webHostBuilder.UseStartup<Startup>();
				})
				.Build();

	}

}