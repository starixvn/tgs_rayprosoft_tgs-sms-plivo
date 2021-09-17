using System;
using System.Threading.Tasks;
using Tgs.Sms.Plivo.Repositories.Entities;

namespace Tgs.Sms.Plivo.Repositories
{
	internal class TgsRepository : ITgsRepository
	{
		private readonly TgsContext _tgsContext;
		private readonly PlivoSerializer _plivoSerializer;

		public TgsRepository(TgsContext tgsContext, PlivoSerializer plivoSerializer)
		{
			_tgsContext = tgsContext ?? throw new ArgumentNullException(nameof(tgsContext));
			_plivoSerializer = plivoSerializer ?? throw new ArgumentNullException(nameof(plivoSerializer));
		}

		public async ValueTask<BrandInfo?> GetBrandInfoByIdAsync(Guid brandId)
		{
			var brand = await _tgsContext.Brands
				.FindAsync(brandId)
				.ConfigureAwait(false);

			if (brand == null)
				return null;

			return new BrandInfo(
				brand.Id,
				brand.Code,
				brand.EnableSms,
				brand.SmsVendorCode,
				string.IsNullOrWhiteSpace(brand.SmsVendorSettings)
					? null
					: _plivoSerializer.Deserialize<PlivoSettings>(brand.SmsVendorSettings));
		}
	}
}
