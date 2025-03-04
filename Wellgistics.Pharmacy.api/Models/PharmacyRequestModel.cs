namespace Wellgistics.Pharmacy.api.Models
{
    public class PharmacyRequestModel
    {
        public PharmacyDetails PharmacyDetails { get; set; }
        public PharmacistInformation PharmacistInformation { get; set; }
        public StoreHours? StoreHours { get; set; }
        public ServiceDetails ServiceDetails { get; set; }
        public InsuranceContractDetails? InsuranceContract { get; set; }
    }

    public class PharmacyDetails
    {
        public string LegalName { get; set; }
        public string LegalBusinessName { get; set; }
        public long Ncpdp { get; set; }
        public long Npi { get; set; }
        public string? DeaNumber { get; set; }
        public DateTime? DeaExpDate { get; set; }
        public string StateLicenseNumber { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public string? MedicareProviderId { get; set; }
        public string? FederalTaxId { get; set; }
        public string? StoreNumber { get; set; }
        public string? DataOrigin { get; set; }
        public State? PmsSystem { get; set; }
        public string? Otherpmssystem { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string? Notes { get; set; }
    }
    public class PharmacistInformation
    {
        public List<Pharmacist> Pharmacists { get; set; }
    }
    public class Pharmacist
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool? IsPharmacyContact { get; set; }
    }
    public class StoreHours
    {
        public List<Day>? Days { get; set; }
        public List<Holiday>? Holidays { get; set; }
        public string? AlternateHours { get; set; }
        public string? AdditionalHolidays { get; set; }
    }

    public class Day
    {
        public string Name { get; set; }
        public bool Open { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
    }

    public class Holiday
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
    }


    public class ServiceDetails
    {
        public List<Service> ServicesOffered { get; set; }
        public GetPropValues? OfferDelivery { get; set; }
        public List<GetPropValues>? DeliveryDays { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? PharmacyDeliveryBoundary { get; set; }
        public GetPropValues? DeliveryServices { get; set; }
        public GetPropValues? DeliveryServiceMethod { get; set; }
        public string? CarrierName { get; set; }
        public string? PharmacyLocationNotes { get; set; }
        public GetPropValues? ShipRxOrders { get; set; }
        public List<GetPropValues>? PreferredShippingCarrier { get; set; }
        public GetPropValues WorkersCompensation { get; set; }
        public GetPropValues MedicaidContracted { get; set; }
        public GetPropValues MedicareContracted { get; set; }
        public string? ContractRestrictionDetails { get; set; }
        public List<State> PharmacyAccreditations { get; set; }
        public List<State>? Gpo { get; set; }
        public List<AdditionalLicenses>? AdditionalLicenses { get; set; }
    }

    public class Service
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    public class Shiping
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class AdditionalLicenses
    {
        public State? State { get; set; }
        public string? LicenseNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class InsuranceContractDetails
    {
        public GetPropValues RetailContract { get; set; }
        public List<State>? InsurancePlans { get; set; }
        public string? otherInsurancePlan { get; set; }        
        public GetPropValues MailOrder { get; set; }
        public List<State>? MailOrderContract { get; set; }
        public string? OtherMailOrderContract { get; set; }
    }
    public class State
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class GetPropValues
    {
        public string Name { get; set; }
        public string Key { get; set; }

    }
    public class PharmacyInfo
    {
        public string LegalName { get; set; }
        public long Npi { get; set; }
        public long Ncpdp { get; set; }
        public string DeaNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string email { get; set; }
    }


}
