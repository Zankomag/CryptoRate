using System;

namespace CryptoRate.Core.Utils {

	public static class EnvironmentWrapper {

		public const string EnvironmentName = "EnvironmentName";

		public static string GetEnvironmentName() => Environment.GetEnvironmentVariable(EnvironmentName);

	}

}