using Wellgistics.Pharmacy.api.Models;

namespace Wellgistics.Pharmacy.api.IService
{
    public interface IPrescriptionService
    {

        Task<RxPharmacyOptions> GetrxPharmacyOptions(string rxTransactionId);
        Task<PharmacyCreationResponse?> CreatePrescriptionAsync(PrescriptionRequestModel prescriptionmodel, string encodedUrl);
        Task<PharmacyCreationResponse?> CreatePrescriptionsAsync(List<PrescriptionRequestModel> prescriptionModel, string encodedUrl);
    }
}
