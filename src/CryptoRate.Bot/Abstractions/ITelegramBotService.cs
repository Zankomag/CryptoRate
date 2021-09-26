using System.Threading.Tasks;
using CoinAPI.REST.V1;
using Telegram.Bot.Types;

namespace CryptoRate.Bot.Abstractions {

	public interface ITelegramBotService {

		Task<Message> SendCurrencyRate(long chatId, Exchangerate currencyRate);

	}

}