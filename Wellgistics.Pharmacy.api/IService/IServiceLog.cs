using Wellgistics.Pharmacy.api.Models;

namespace Wellgistics.Pharmacy.api.IService
{
    public interface IServiceLogService
    {
        Task LogServiceCallAsync(ServiceLogModel serviceLogModel);

    }
}
