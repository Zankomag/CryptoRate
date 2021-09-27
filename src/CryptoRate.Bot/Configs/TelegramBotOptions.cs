
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace CryptoRate.Bot.Configs {

	public class TelegramBotOptions {

		public const string SectionName = "TelegramBotClient";
		
		public string Token { get; init;  }
		public long[] AdminIds { get; init;  }

		public string RedStickerFileId { get; init;  }
		public string GreenStickerFileId { get; init;  }

	}

}