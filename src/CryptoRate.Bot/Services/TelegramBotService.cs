using System;
using System.Threading.Tasks;
using CoinAPI.REST.V1;
using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using CryptoRate.Core.Abstractions;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using System.Linq;
using System.Threading;
using CryptoRate.Common.Utils;
using Telegram.Bot.Extensions.Polling;

// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace CryptoRate.Bot.Services {

//TODO add inline mode for getting rate in any chat
	public class TelegramBotService : ITelegramBotService {

		//Markdown template
		private const string currencyRateMessageTemplate = "*₿* = `${0:0.00}`\n\n_{1}_";
		private readonly TelegramBotClient client;
		private readonly ICryptoClient cryptoClient;
		private readonly DateTime startTime;

		private readonly TelegramBotOptions options;

		public TelegramBotService(IOptions<TelegramBotOptions> telegramBotOptions, ICryptoClient cryptoClient) {
			this.cryptoClient = cryptoClient;
			options = telegramBotOptions.Value;
			client = new TelegramBotClient(options.Token);
			startTime = DateTime.UtcNow;
		}

		//TODO Add Async suffix here and everywhere we need
		public async Task<Message> SendCurrencyRate(long chatId, Exchangerate currencyRate) {
			string message = GetCurrencyRateMessage(currencyRate);
			
			var result = await client.SendTextMessageAsync(chatId, message, ParseMode.Markdown);
			return result;
		}
		
		private static string GetCurrencyRateMessage(Exchangerate currencyRate) 
			=> currencyRate != null
				? String.Format(currencyRateMessageTemplate, currencyRate.rate, currencyRate.time.Date.ToString("dd/MM/yyyy"))
				: "Sorry, I've received an error from CoinAPI. Make sure limits are not drained.";

		public async Task HandleMessageAsync(Message message) {
			//bot doesn't read old messages to avoid /*spam*/ 
			// DISABLED DUE TO LAMBDA ISSUES
			//TODO fix end enable
			//if(message.Date < startTime) return;

			//If command contains bot username we need to exclude it from command (/btc@MyBtcBot should be /btc)
			int atIndex = message.Text.IndexOf('@');
			string command = atIndex == -1 ? message.Text : message.Text[..atIndex];

			switch(command.ToLower()) {
				case "/btc":
				case "/btctousd":
					var currencyRate = await cryptoClient.GetBtcToUsdCurrencyRate();
					await SendCurrencyRate(message.Chat.Id, currencyRate);
					break;
				case "/health":
					await client.SendTextMessageAsync(message.From.Id, "Running, Environment: " + EnvironmentWrapper.GetEnvironmentName() + "\nstart time: " + startTime);
					break;
			}
		}

		public async Task HandleInlineQueryAsync(InlineQuery inlineQuery) {
			Random random = new Random();
			if(options.AdminIds.Contains(inlineQuery.From.Id)) {
				var currencyRate = await cryptoClient.GetBtcToUsdCurrencyRate();
				await client.AnswerInlineQueryAsync(inlineQuery.Id,
					new[] {
						new InlineQueryResultCachedSticker(Guid.NewGuid().ToString(), random.Next(0, 2) > 0 ? options.GreenStickerFileId : options.RedStickerFileId)
							{InputMessageContent = new InputTextMessageContent(GetCurrencyRateMessage(currencyRate)) {ParseMode = ParseMode.Markdown}}
					});
			}
		}

		/// <inheritdoc />
		async Task IUpdateHandler.HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) => await HandleUpdateAsync(update);
		
		public async Task HandleUpdateAsync(Update update) {
			switch(update.Type) {
				case UpdateType.Message:
					if(update.Message.Type == MessageType.Text) await HandleMessageAsync(update.Message);
					break;
				case UpdateType.InlineQuery:
					await HandleInlineQueryAsync(update.InlineQuery);
					break;
				default: throw new InvalidOperationException($"Update type {update.Type} is not supported");
			}

		}

		/// <inheritdoc />
		public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
			//TODO Log exception
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public UpdateType[] AllowedUpdates { get; } = {UpdateType.Message, UpdateType.InlineQuery};

	}

}