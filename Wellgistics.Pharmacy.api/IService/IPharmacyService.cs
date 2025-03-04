using Microsoft.AspNetCore.Mvc;
using Wellgistics.Pharmacy.api.Models;

namespace Wellgistics.Pharmacy.api.IService
{
    public interface IPharmacyService
    {
        Task<PharmacyCreationResponse?> CreatePharmacyAsync(PharmacyRequestModel ruleSetRequestModel);
        Task<PharmacyInstance> GetPharmaciesByIdentifiers(string? nationalProviderId, string? ncpdp, string? medicareProviderId);
        Task<List<ConfigurationType>> GetConfigureNamesByType(string type);
        Task<List<Prescription>> GetPharmacyPrescriptionsByStatus(string status, string pharmacyInfoId);
        Task<int> UpdatePharmacyPrescriptionStatus(string status, string Id, string updatedBy, string rxPharmacyInfoId, string encodedUrl);
        Task<PrescriptionStausCount> GetPharmacyPrescriptionsStatusCounts(string pharmacyId);
        Task<UserProfile> GetUserProfile(string email);
        Task<PharmacyCreationResponse> Createorupdatepharmacy([FromBody] PharmacyInfo pharmacyInfo);
        Task<PositionStackAddress> ValidateAddressAsync(AddressModel addressModel);


    }
}
