using System.ComponentModel.DataAnnotations;

namespace CryptoRate.Core.Configs {

	public class CryptoClientOptions {

		public const string SectionName = "CryptoClient";

		[Required]
		[RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
		public string ApiKey { get; init; }

	}

}