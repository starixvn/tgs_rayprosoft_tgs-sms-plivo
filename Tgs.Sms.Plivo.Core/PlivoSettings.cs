using System.Text.Json.Serialization;

namespace Tgs.Sms.Plivo
{
	public record PlivoSettings
	{
		[JsonPropertyName("authId")]
		public string AuthId { get; init; } = default!;

		[JsonPropertyName("authToken")]
		public string AuthToken { get; init; } = default!;
	};
}
