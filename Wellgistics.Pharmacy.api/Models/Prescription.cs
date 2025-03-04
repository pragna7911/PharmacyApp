using Microsoft.EntityFrameworkCore;

namespace Wellgistics.Pharmacy.api.Models
{
    [Keyless]
    public class Prescription
    {
        public string RxId { get; set; }
        public string DrugName { get; set; }
        public string DeliveryType { get; set; }
        public long? DAW { get; set; }
        public int? Quantity { get; set; }
        public int? Id {  get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpirationDate {  get; set; }
        public string? Insurance { get; set; }
        public long? NCPDP { get; set; }
        public string? DrugNDC { get; set; }
        public string? EncodedUrl { get; set; }
        public string? UpdatedBy { get; set; }
        public int? RxTransactionId { get; set; }
        public int? RxPharmacyInfoId { get; set; }




    }

    [Keyless]
    public class PrescriptionStausCount
    {
        public int InQueueCount { get; set; }
        public int InProcessCount { get; set; }
        public int RejectandExpiredCount { get; set; }
        public int CompletedCount { get; set; }
        public int SoonExpiryCount { get; set; }
        public int SLAInprocessCount { get; set; }
     

    }
    [Keyless]
    public class PrescriptionUpdateStatus
    {
        public int? RxTransactionId { get; set; }
        public int? Status { get; set; }
        public string? EncodedUrl { get; set;}
    }

    public class PrescriptionRequestModel
    {
        public string RxId { get; set; }
        public string RxDrugName { get; set; }
        public string RxDeliveryMethod { get; set; }
        public long RxDAW { get; set; }
        public int RxQuantity { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? RxInsurance { get; set; }
        public long RxPharmacyNCPDP { get; set; }
        public string RxSourceNDC { get; set; }
        //public string EncodedUrl { get; set; }
        public string? UpdatedBy { get; set; }
        public int? RxTransactionId { get; set; }
        public int? RxPharmacyInfoId { get; set; }



    }

    [Keyless]
    public class RxOptions
    {
        public string OptionText { get; set; }
        public string OptionName { get; set; }
        public string RxTransactionId { get; set; }
        public int PrescriptionOptionId { get; set; }
        public int PatientInfoId { get; set; }
    }

    [Keyless]
    public class RxPharmacy
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public int RxPharmacyInfoId { get; set; }
    }

    [Keyless]
    public class RxPharmacyOptions
    {
        public List<RxPharmacy> pharmacies { get; set; }
        public List<RxOptions> Options { get; set; }
    }

    [Keyless]
    public class RxResponse
    {
        public int? RxPharmacyInfoId { get; set; }
        public bool IsSelected { get; set; }

        public int? PrescriptionOptionId { get; set; }
        public string? RxTransactionId { get; set; }
        public int? PatientInfoId { get; set; }
        public string? PatientLastName { get; set; }
        public string? PatientDOB { get; set; }
        public string? PatientPhone { get; set; }
        public bool IsDefaultPharmacy { get; set; }
        public int? PharmacyInfoId { get; set; }

    }



}
