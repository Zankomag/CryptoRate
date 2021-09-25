using System.ComponentModel.DataAnnotations;

namespace CryptoRate.Core.Configs {

	public class CryptoClientOptions {

		[Required]
		[RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
		public string ApiKey { get; set; }

	}

}