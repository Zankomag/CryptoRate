using System;
using System.Threading.Tasks;
using CoinAPI.REST.V1;
using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CryptoRate.Bot.Services {

//TODO add inline mode for getting rate in any chat
	public class TelegramBotService : ITelegramBotService {

		//Markdown template
		private const string currencyRateMessageTemplate = "*₿* =  `${0:0.00}`\n\n_{1}_";
		private readonly TelegramBotClient client;

		private readonly TelegramBotOptions options;

		public TelegramBotService(IOptions<TelegramBotOptions> telegramBotOptions) {
			options = telegramBotOptions.Value;
			client = new TelegramBotClient(options.Token);
		}

		public async Task<Message> SendCurrencyRate(long chatId, Exchangerate currencyRate) {
			string message = currencyRate != null
				? String.Format(currencyRateMessageTemplate, currencyRate.rate, currencyRate.time.Date.ToString("dd/MM/yyyy"))
				: "Sorry, I've received an error from CoinAPI. Make sure limits are not drained.";
			
			var result = await client.SendTextMessageAsync(chatId, message, ParseMode.Markdown);
			return result;
		}

	}

}