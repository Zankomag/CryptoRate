using System.Threading.Tasks;
using CryptoRate.Bot.Abstractions;
using CryptoRate.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace CryptoRate.Bot.Controllers {

	[Route("api/[controller]")]
	public class BotUpdateController : ControllerBase {

		private readonly ITelegramBotService telegramBotService;

		public BotUpdateController(ITelegramBotService telegramBotService) {
			this.telegramBotService = telegramBotService;
		}

		[HttpPost("{token}")]
		public async Task<IActionResult> PostUpdate([FromBody] Update update, string token) {
			//TODO check token
			await telegramBotService.HandleUpdateAsync(update);
			return Ok();
		}

		[HttpGet("healthCheck")]
		public string HealthCheck() => EnvironmentWrapper.GetEnvironmentName();

	}

}