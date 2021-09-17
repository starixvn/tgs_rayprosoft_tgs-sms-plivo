using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Plivo;
using Tgs.MessageQueue;
using Tgs.Sms.Protos;

namespace Tgs.Sms.Plivo.MessageQueue
{
	public class PlivoReceiveHandler : IMessageHandler
	{
		private readonly ITgsRepository _tgsRepository;
		private readonly ILogger<PlivoReceiveHandler> _logger;

		public PlivoReceiveHandler(ITgsRepository tgsRepository, ILogger<PlivoReceiveHandler> logger)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async ValueTask HandleAsync(byte[] data, CancellationToken cancellationToken = default)
		{
			var shortMsg = ShortMessage.Parser.ParseFrom(data);
			var brandId = shortMsg.BrandId.ToGuid();

			_logger.LogInformation($"Receive sms: [{nameof(shortMsg.BrandId)}] {brandId} " +
				$"[{nameof(shortMsg.Destination)}] {shortMsg.Destination} " +
				$"[{nameof(shortMsg.Content)}] {shortMsg.Content}");

			var brandInfo = await _tgsRepository
				.GetBrandInfoByIdAsync(brandId)
				.ConfigureAwait(false);

			if (brandInfo == null)
			{
				_logger.LogInformation($"Can't find brand info ({brandId})");

				return;
			}

			if (!brandInfo.SmsEnabled || brandInfo.SmsVendorSettings == null)
			{
				_logger.LogWarning("Brand setting warn");

				return;
			}

			var api = new PlivoApi(
				brandInfo.SmsVendorSettings.AuthId,
				brandInfo.SmsVendorSettings.AuthToken);

			var response = await api.Message
				.CreateAsync(
					src: brandInfo.Name,
					dst: new List<string> { shortMsg.Destination },
					text: shortMsg.Content)
				.ConfigureAwait(false);

			if (!(response.StatusCode >= 200 && response.StatusCode < 300))
				throw new Exception("Sms send fail");

			_logger.LogInformation("Sms sent.");
		}
	}
}
