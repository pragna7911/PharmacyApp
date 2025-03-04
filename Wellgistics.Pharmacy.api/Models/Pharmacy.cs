using System.Security.Cryptography;
using System;
using Microsoft.EntityFrameworkCore;

namespace Wellgistics.Pharmacy.api.Models
{
    [Keyless]
    public class PharmacyInstance
    {
        public int PharmacyInfoId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LegalName { get; set; }
        public string? LegalBusinessName { get; set; }
        public long? NCPDP { get; set; }
        public long? NPI { get; set; }
        public string? DEA { get; set; }
        public DateTime? DeaExpirationDate { get; set; }
        public string? StateLicenseNumber { get; set; }
        public DateTime? StateLicenseExpiryDate { get; set; }
        public string? MedicareProviderId { get; set; }
        public string? FederalTaxId { get; set; }
        public string? StoreNumber { get; set; }
        public string? DataOrigin { get; set; }
        public string? PmsSystem { get; set; }
        public string? OtherPmsSystem { get; set; }
        public string? AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? Notes { get; set; }
        //public string? PharmacistInfo { get; set; }
        //public string? StoreHours { get; set; }
        public string? ServicesOffered { get; set; }
        public bool? IsOfferDelivery { get; set; }
        public string? DeliveryDays { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? DeliveryBoundary { get; set; }
        public string? DeliveryServiceMethod { get; set; }
        public string? PreferredShippingCarrier { get; set; }
        public bool? DeliveryServices { get; set; }  // Updated to match alias
        public string? PharmacyLocationNotes { get; set; }  // Updated to match alias
        public bool? ShipRxOrders { get; set; }  // Updated to match alias
        public bool? WorkersCompensation { get; set; }  // Updated to match alias
        public bool? MedicaidContracted { get; set; }  // Updated to match alias
        public bool? MedicareContracted { get; set; }  // Updated to match alias
        public string? ContractRestrictionDetails { get; set; }
        public string? Accreditations { get; set; }
        public string? Gpo { get; set; }
        //public string? AdditionalLicenses { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }


    }

    public class PharmacyRequestModel1
    {
        public DateTime CreatedDate { get; set; }
        public Guid PharmacyUuid { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string FederalTaxIdNumber { get; set; }
        public string DataOrigin { get; set; }
        public DateTime? DeaExpirationDate { get; set; }
        public string MedicareProviderId { get; set; }
        public string StoreNumber { get; set; }
        public bool Status { get; set; }  // Use bool for bit(1)
        public string PmsSystem { get; set; }
        public string Notes { get; set; }
        public string Gpo { get; set; }
        public string StateLicenseNumber { get; set; }
        public DateTime StateLicenseExpiryDate { get; set; }
        public string StoreHours { get; set; }
        public string DeliveryDriver { get; set; }
        public string DeliveryBoundary { get; set; }
        public string DeliverySchedule { get; set; }
        public string LocationNotes { get; set; }
        public bool ShipRxOrdersForPatientsAcrossYourState { get; set; }  // Use bool for bit(1)
        public string PreferredShippingCarrier { get; set; }
        public string HolidaysClosed { get; set; }
        public string AlternateHours { get; set; }
        public string AdditionalHolidaysClosed { get; set; }
        public string Timezone { get; set; }
        public string PharmacyType { get; set; }
        public string PhoneNumber { get; set; }
        public string PharmacyEmployee { get; set; }
        public bool ServicesUnderStateMedicaid { get; set; }  // Use bool for bit(1)
        public bool ServicesUnderMedicarePartB { get; set; }  // Use bool for bit(1)
        public bool IsWorkersCompensationProgram { get; set; }  // Use bool for bit(1)
        public string AdditionalLicenses { get; set; }
        public string Services { get; set; }
        public string Accreditations { get; set; }
        public string PSAO { get; set; }
        public string ContractRestrictionDetails { get; set; }
        public string WholeSalersInfo { get; set; }
        public bool ReportingToIQVIA { get; set; }  // Use bool for bit(1)
        public string NationalProviderId { get; set; }
        public string NCPDP { get; set; }
        public string DEA { get; set; }
        public string PharmacyName { get; set; }
        public string LegalBusinessName { get; set; }
        public string PmsOutBoundTransferMethod { get; set; }
        public string State { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string NPI { get; set; }
        public string OtherPMSSystem { get; set; }
        public bool IsOfferDelivery { get; set; }
        public string DeliveryDays { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string DeliveryServiceMethod { get; set; }
        public bool UseExistingDeliveryService { get; set; }
        public bool IsRetail { get; set; }
        public bool IsMailOrder { get; set; }
        public bool IsCompounding { get; set; }
        public string LegalName { get; set; }
        public string PostalCode { get; set; }
    }
    public class PharmacyCreationResponse
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
    }
    [Keyless]
    public class ConfigurationType
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
