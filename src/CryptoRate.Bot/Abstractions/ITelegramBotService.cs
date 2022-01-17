using System.Threading.Tasks;
using CoinAPI.REST.V1;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace CryptoRate.Bot.Abstractions {

	//TODO add unit and integration tests
	public interface ITelegramBotService : IUpdateHandler {

		Task<Message> SendCurrencyRateAsync(long chatId, Exchangerate currencyRate, string baseCurrencyChar, string quoteCurrencyChar);
		Task<Message> SendCurrencyRateAsync(long chatId, string currencyBase, string currencyQuote);

		Task HandleUpdateAsync(Update update);

		bool IsTokenCorrect(string token);

	}

}