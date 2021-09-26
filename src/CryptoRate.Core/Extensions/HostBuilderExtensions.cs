using System;
using System.IO;
using System.Reflection;
using CryptoRate.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoRate.Core.Extensions {

	/// <summary>
	///     Extensions to emulate a typical "Startup.cs" pattern for <see cref="IHostBuilder" />
	/// </summary>
	public static class HostBuilderExtensions {

		private const string configureServicesMethodName = "ConfigureServices";

		/// <summary>
		///     Specify the startup type to be used by the host.
		/// </summary>
		/// <typeparam name="TStartup">
		///     The type containing an optional constructor with
		///     an <see cref="IConfiguration" /> parameter. The implementation should contain a public
		///     method named ConfigureServices with <see cref="IServiceCollection" /> parameter.
		/// </typeparam>
		/// <param name="hostBuilder">The <see cref="IHostBuilder" /> to initialize with TStartup.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder" /> for chaining.</returns>
		public static IHostBuilder UseStartup<TStartup>(this IHostBuilder hostBuilder) where TStartup : class {
			// Invoke the ConfigureServices method on IHostBuilder...
			hostBuilder.ConfigureServices((ctx, serviceCollection) => {
				// Find a method that has this signature: ConfigureServices(IServiceCollection)
				var cfgServicesMethod = typeof(TStartup).GetMethod(configureServicesMethodName, new[] {typeof(IServiceCollection)});

				// Check if TStartup has a ctor that takes a IConfiguration parameter
				var hasConfigCtor = typeof(TStartup).GetConstructor(new[] {typeof(IConfiguration)}) != null;

				// create a TStartup instance based on ctor
				var startUpObj = hasConfigCtor ? (TStartup)Activator.CreateInstance(typeof(TStartup), ctx.Configuration) : (TStartup)Activator.CreateInstance(typeof(TStartup), null);

				// finally, call the ConfigureServices implemented by the TStartup object
				cfgServicesMethod?.Invoke(startUpObj, new object[] {serviceCollection});
			});

			hostBuilder.AddConfiguration<TStartup>();
			
			// chain the response
			return hostBuilder;
		}

		/// <summary>
		/// Sets Hotsing Environment
		/// Adds config from appsettings.json and appsettings.Environment.json files
		/// Loads User Secrets if Development
		/// </summary>
		/// <param name="hostBuilder"></param>
		/// <returns></returns>
		public static IHostBuilder AddConfiguration<TStartup>(this IHostBuilder hostBuilder) {
			var projectDir = GetProjectPath<TStartup>("");
			hostBuilder.UseContentRoot(projectDir);
			hostBuilder.ConfigureAppConfiguration((hostingContext, configurationBuilder) => {
				configurationBuilder.AddEnvironmentVariables();
				hostingContext.HostingEnvironment.EnvironmentName = EnvironmentWrapper.GetEnvironmentName();
				
				configurationBuilder.AddJsonFile("appsettings.json", false)
					.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", false);

				if(hostingContext.HostingEnvironment.IsDevelopment() && !String.IsNullOrEmpty(hostingContext.HostingEnvironment.ApplicationName)) {
					var appAssembly = Assembly.Load(new AssemblyName(hostingContext.HostingEnvironment.ApplicationName));
					configurationBuilder.AddUserSecrets(appAssembly, true);
				}
			});
			return hostBuilder;
		}

		/// Ref: https://stackoverflow.com/a/50581752/11101834
		/// Ref: https://stackoverflow.com/a/52136848/3634867
		/// <summary>
		/// Gets the full path to the target project that we wish to test
		/// </summary>
		/// <param name="projectRelativePath">
		/// The parent directory of the target project.
		/// e.g. src, samples, test, or test/Websites
		/// </param>
		/// <param name="startupAssembly">The target project's assembly.</param>
		/// <returns>The full path to the target project.</returns>
		private static string GetProjectPath<TStartup>(string projectRelativePath) {
			var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
			
			// Get name of the target project which we want to test
			var projectName = startupAssembly.GetName().Name;

			// Get currently executing test project path
			var applicationBasePath = System.AppContext.BaseDirectory;

			// Find the path to the target project
			var directoryInfo = new DirectoryInfo(applicationBasePath);
			do {
				directoryInfo = directoryInfo.Parent;

				var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));
				if(projectDirectoryInfo.Exists) {
					var projectFileInfo = new FileInfo(Path.Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
					if(projectFileInfo.Exists) {
						return Path.Combine(projectDirectoryInfo.FullName, projectName);
					}
				}
			} while(directoryInfo.Parent != null);

			throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
		}

	}

}