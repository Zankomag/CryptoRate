using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using CryptoRate.Core.Abstractions;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CryptoRate.Bot.Services {

	public class TelegramBotLocalHostedService : ITelegramBotLocalHostedService {

		private readonly ITelegramBotService telegramBotService;
		private readonly ICryptoClient cryptoClient;
		private readonly ITelegramBotClient client;
		private readonly DateTime startTime;

		private readonly TelegramBotOptions options;
		private CancellationTokenSource cancellationTokenSource;
		private Task pollingTask;

		public TelegramBotLocalHostedService(IOptions<TelegramBotOptions> telegramBotOptions, ITelegramBotService telegramBotService, ICryptoClient cryptoClient) {
			this.telegramBotService = telegramBotService;
			this.cryptoClient = cryptoClient;
			options = telegramBotOptions.Value;
			client = new TelegramBotClient(options.Token);
			startTime = DateTime.UtcNow;
		}

		/// <inheritdoc />
		public Task StartAsync(CancellationToken cancellationToken) {
			cancellationTokenSource = new CancellationTokenSource();
			pollingTask = Task.Run(() => client.ReceiveAsync(this, cancellationTokenSource.Token), cancellationToken);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public async Task StopAsync(CancellationToken cancellationToken) {
			cancellationTokenSource.Cancel();
			await pollingTask;
		}

		/// <inheritdoc />
		public async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
			switch(update.Type) {
				case UpdateType.Message:
					if(update.Message.Type == MessageType.Text) await HandleMessageAsync(update.Message);
					break;
				case UpdateType.InlineQuery:
					//HandleInlineQueryAsync
					break;
			}

		}

		/// <inheritdoc />
		public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
			//TODO log exception
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public UpdateType[] AllowedUpdates { get; } = {UpdateType.Message, UpdateType.InlineQuery};

		private async Task HandleMessageAsync(Message message) {
			//bot doesn't read old messages to avoid /*spam*/ 
			if(message.Date < startTime) return;
			//If command contains bot username we need to exclude it from command (/btc@MyBtcBot should be /btc)
			int atIndex = message.Text.IndexOf('@');
			string command = atIndex == -1 ? message.Text : message.Text[..atIndex];

			switch(command.ToLower()) {
				case "/btc":
				case "/btctousd":
					var currencyRate = await cryptoClient.GetCurrencyRate("BTC", "USD");
					await telegramBotService.SendCurrencyRate(message.Chat.Id, currencyRate);
					break;
			}
		}

	}

}