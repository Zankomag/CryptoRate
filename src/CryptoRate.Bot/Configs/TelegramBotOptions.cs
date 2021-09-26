﻿using CryptoRate.Bot.Services;

namespace CryptoRate.Bot.Configs {

	public class TelegramBotOptions {

		public const string SectionName = "TelegramBotClient";
		
		public string Token { get; set; }
		public long[] AdminIds { get; set; }

	}

}