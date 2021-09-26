using System.Threading;
using System.Threading.Tasks;
using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using CryptoRate.Core.Abstractions;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace CryptoRate.Bot.Services {

	public class TelegramBotLocalHostedService : ITelegramBotLocalHostedService {

		private readonly ITelegramBotService telegramBotService;
		private readonly ICryptoClient cryptoClient;
		private readonly ITelegramBotClient client;

		private readonly TelegramBotOptions options;

		private CancellationTokenSource cancellationTokenSource;
		private Task pollingTask;


		public TelegramBotLocalHostedService(IOptions<TelegramBotOptions> telegramBotOptions, ITelegramBotService telegramBotService, ICryptoClient cryptoClient) {
			this.telegramBotService = telegramBotService;
			this.cryptoClient = cryptoClient;
			options = telegramBotOptions.Value;
			client = new TelegramBotClient(options.Token);
		}

		/// <inheritdoc />
		public Task StartAsync(CancellationToken cancellationToken) {
			cancellationTokenSource = new CancellationTokenSource();
			pollingTask = Task.Run(() => client.ReceiveAsync(telegramBotService, cancellationTokenSource.Token), cancellationToken);
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public async Task StopAsync(CancellationToken cancellationToken) {
			cancellationTokenSource.Cancel();
			await pollingTask;
		}

	}

}