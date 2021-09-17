using System;

namespace Tgs.Sms.Plivo
{
	public record BrandInfo(
		Guid Id,
		string Name,
		bool SmsEnabled,
		string? SmsVendorCode,
		PlivoSettings? SmsVendorSettings);
}
