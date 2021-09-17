using System;
using System.Threading.Tasks;

namespace Tgs.Sms.Plivo
{
    public interface ITgsRepository
    {
        public ValueTask<BrandInfo?> GetBrandInfoByIdAsync(Guid brandId);
    }
}
