using System.Threading.Tasks;
using CoinAPI.REST.V1;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace CryptoRate.Bot.Abstractions {

	public interface ITelegramBotService : IUpdateHandler {

		Task<Message> SendCurrencyRate(long chatId, Exchangerate currencyRate);

		Task HandleMessageAsync(Message message);

		Task HandleInlineQueryAsync(InlineQuery inlineQuery);

	}

}