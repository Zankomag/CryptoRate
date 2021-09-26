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

		private TelegramBotOptions options;
		private readonly TelegramBotClient client;

		//Markdown template
		private const string currencyRateMessageTemplate = "*₿* =  `${0:0.00}`\n\n_{1}_";
		
		public TelegramBotService(IOptions<TelegramBotOptions> telegramBotOptions) {
			options = telegramBotOptions.Value;
			client = new TelegramBotClient(options.Token);
		}

		public async Task<Message> SendCurrencyRate(long chatId, Exchangerate currencyRate) {
			string message = String.Format(currencyRateMessageTemplate, currencyRate.rate, currencyRate.time.Date.ToString("MM/dd/yyyy"));
			try {
				var result = await client.SendTextMessageAsync(chatId, message, ParseMode.MarkdownV2);
				return result;
			} catch(Exception ex) {
				//TODO log exception
				throw;
			}
		}

	}

}