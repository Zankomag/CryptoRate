using System.ComponentModel.DataAnnotations;
using CryptoRate.Core.Services;

namespace CryptoRate.Core.Configs {

	public class CryptoClientOptions {

		public const string SectionName = nameof(CryptoClient);

		[Required]
		[RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
		public string ApiKey { get; set; }

	}

}