using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoinAPI.REST.V1;
using CryptoRate.Bot.Abstractions;
using CryptoRate.Bot.Configs;
using CryptoRate.Common.Utils;
using CryptoRate.Core.Abstractions;
using CryptoRate.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace CryptoRate.Bot.Services {
	
	public class TelegramBotService : ITelegramBotService {
		
		private readonly TelegramBotClient client;
		private readonly ICryptoClient cryptoClient;
		private readonly ILogger<TelegramBotService> logger;
		private readonly DateTime startDateTime;
		private readonly TelegramBotOptions options;

		/// <summary>
		/// Bot username with @ in front
		/// </summary>
		private readonly Lazy<Task<string>> botUsername;

		/// <inheritdoc />
		public UpdateType[] AllowedUpdates { get; } = { UpdateType.Message, UpdateType.InlineQuery };

		public TelegramBotService(IOptions<TelegramBotOptions> telegramBotOptions, ICryptoClient cryptoClient, ILogger<TelegramBotService> logger) {
			this.cryptoClient = cryptoClient;
			this.logger = logger;
			options = telegramBotOptions.Value;
			client = new TelegramBotClient(options.Token);
			botUsername = new Lazy<Task<string>>(async () => await InitializeBotUsername());
			startDateTime = DateTime.UtcNow;
		}

		private async Task<string> InitializeBotUsername() {
			var botInfo = await client.GetMeAsync();
			return String.Concat('@', botInfo.Username);
		}
		
		public bool IsTokenCorrect(string token) => token != null && token == options.Token;
		
		public async Task HandleMessageAsync(Message message) {
			//bot doesn't read old messages to avoid /*spam*/ 
			//2 minutes threshold due to slow start of aws lambda
			if(message.Date < startDateTime.AddMinutes(-2)) return;

			//If command contains bot username we need to exclude it from command (/btc@MyBtcBot should be /btc)
			int atIndex = message.Text.IndexOf('@');

			string username = await botUsername.Value;
			//Bot should not respond to commands in group chats without direct mention
			if(message.From.Id != message.Chat.Id && atIndex != -1 && message.Text[(atIndex + 1)..] != username) 
				return;
			
			string command = atIndex == -1 ? message.Text : message.Text[..atIndex];

			//Command handler has such a simple and dirty implementation because telegram bot is really simple and made mostly for demonstration purpose
			switch(command.ToLower()) {
				case "/btc":
				case "/btctousd":
					await SendCurrencyRateAsync(message.Chat.Id, CurrencyCode.Bitcoin, CurrencyCode.Usd);
					break;
				case "/eth":
				case "/ethtousd":
					await SendCurrencyRateAsync(message.Chat.Id, CurrencyCode.Ethereum, CurrencyCode.Usd);
					break;
				case "/health":
				case "/status":
					if(options.IsUserAdmin(message.From.Id)) {
						await client.SendTextMessageAsync(message.From.Id, $"Running\nEnvironment: {EnvironmentWrapper.GetEnvironmentName()}\ndotnet {Environment.Version}\nstart time: {startDateTime}");
					}
					break;
			}
		}

		public async Task<Message> SendCurrencyRateAsync(long chatId, string currencyBase, string currencyQuote)
			=> await SendCurrencyRateAsync(chatId,
				await cryptoClient.GetCurrencyRateAsync(currencyBase, currencyQuote),
				currencyBase.GetCurrencyCharByCode(),
				currencyQuote.GetCurrencyCharByCode());

		public async Task HandleInlineQueryAsync(InlineQuery inlineQuery) {
			if(options.IsUserAdmin(inlineQuery.From.Id)) {
				Random random = new();
				string baseCurrency, quoteCurrency;
				if(String.IsNullOrWhiteSpace(inlineQuery.Query)) {
					baseCurrency = CurrencyCode.Bitcoin;
					quoteCurrency = CurrencyCode.Usd;
				} else {
					var currencyCodes = inlineQuery.Query.Split(' ');
					baseCurrency = currencyCodes[0].ToUpper();
					quoteCurrency = currencyCodes.Length > 1 ? currencyCodes[1].ToUpper() : CurrencyCode.Usd;
				}
				
				var currencyRate = await cryptoClient.GetCurrencyRateAsync(baseCurrency, quoteCurrency);
				await client.AnswerInlineQueryAsync(inlineQuery.Id,
					new[] {
						new InlineQueryResultCachedSticker(Guid.NewGuid().ToString(),
							random.Next(0, 2) > 0 ? options.GreenStickerFileId : options.RedStickerFileId) {
							InputMessageContent = new InputTextMessageContent(GetCurrencyRateMessage(currencyRate,
									baseCurrency.GetCurrencyCharByCode(), quoteCurrency.GetCurrencyCharByCode()))
								{ParseMode = ParseMode.Markdown}
						}
					}, 10, true);
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
				default: logger.LogWarning("Update type {update.Type} is not supported", update.Type);
					break;
			}

		}

		/// <inheritdoc />
		public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
			logger.LogError(exception, "Received an exception from Telegram Bot API");
			return Task.CompletedTask;
		}
		
		public async Task<Message> SendCurrencyRateAsync(long chatId, Exchangerate currencyRate, string baseCurrencyChar, string quoteCurrencyChar) {
			string message = GetCurrencyRateMessage(currencyRate, baseCurrencyChar, quoteCurrencyChar);

			var result = await client.SendTextMessageAsync(chatId, message, ParseMode.Markdown);
			return result;
		}

		private string GetCurrencyRateMessage(Exchangerate currencyRate, string baseCurrencyChar, string quoteCurrencyChar) {
			if(currencyRate != null) {
				string currencySignAndAmountSeparator = null;

				//This is used to make output more convenient if there is no char for currency code (USD500 vs USD 500)
				if(quoteCurrencyChar == currencyRate.asset_id_quote)
					currencySignAndAmountSeparator = " ";
				return String.Format(options.CurrencyRateMarkdownMessageTemplate,
					baseCurrencyChar,
					quoteCurrencyChar,
					currencyRate.rate,
					currencyRate.time.Date.ToString("dd/MM/yyyy"),
					currencySignAndAmountSeparator);
			}
			return "Sorry, I've received an error from CoinAPI. Make sure limits are not drained.";

			//TODO Add handler if currency code in request was wrong (in crypto client)
		}

	}

}