using System.Threading;
using System.Threading.Tasks;
using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace CryptoRate.Bot.Services {

	public class TelegramBotLocalHostedService : ITelegramBotLocalHostedService {

		private readonly ITelegramBotService telegramBotService;
		private readonly ITelegramBotClient client;

		private CancellationTokenSource cancellationTokenSource;
		private Task pollingTask;

		public TelegramBotLocalHostedService(IOptions<TelegramBotOptions> telegramBotOptions, ITelegramBotService telegramBotService) {
			this.telegramBotService = telegramBotService;
			TelegramBotOptions options = telegramBotOptions.Value;
			client = new TelegramBotClient(options.Token);
		}

		/// <inheritdoc />
		public Task StartAsync(CancellationToken cancellationToken) {
			//TODO Log ("Starting telegram polling...");
			cancellationTokenSource = new CancellationTokenSource();
			pollingTask = Task.Run(() => client.ReceiveAsync(telegramBotService, cancellationTokenSource.Token), cancellationToken);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public async Task StopAsync(CancellationToken cancellationToken) {
			//TODO Log ("Stopping telegram polling...");
			cancellationTokenSource.Cancel();
			await pollingTask;
		}

	}

}