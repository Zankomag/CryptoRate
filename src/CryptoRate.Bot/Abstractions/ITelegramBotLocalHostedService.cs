using Microsoft.Extensions.Hosting;
using Telegram.Bot.Extensions.Polling;

namespace CryptoRate.Bot.Abstractions {

	public interface ITelegramBotLocalHostedService : IUpdateHandler, IHostedService { }

}