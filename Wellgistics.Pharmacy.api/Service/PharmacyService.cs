using System.Data;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Wellgistics.Pharmacy.api.Common;
using Wellgistics.Pharmacy.api.IRepositries;
using Wellgistics.Pharmacy.api.IService;
using Wellgistics.Pharmacy.api.Models;
using Wellgistics.Pharmacy.api.Repository;

namespace Wellgistics.Pharmacy.api.Service
{
    public class PharmacyService : IPharmacyService
    {
        private readonly IRepository _repository;
        private readonly HttpClientHelper _httpClientHelper;
        private readonly IConfiguration _configuration;
        private readonly IStackyonService _stackyonService;
        private readonly PharmacyDbContext _context;
        private readonly DelivmedsDbContext _Einsteincontext;
        public PharmacyService(IRepository repository, HttpClientHelper httpClientHelper, IConfiguration configuration, IStackyonService stackyonService, PharmacyDbContext context, DelivmedsDbContext einsteincontext)
        {
            _repository = repository;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            _stackyonService = stackyonService;
            _context = context;
            _Einsteincontext = einsteincontext;
        }
        //public async Task<PharmacyCreationResponse?> CreatePharmacyAsync(PharmacyRequestModel pharmacyRequestModel)
        //{
        //    try
        //    {
        //        // Create the parameters for the stored procedure
        //        var parameters = new[]
        //        {
        //    new MySqlParameter("@P_CreatedDate", pharmacyRequestModel.CreatedDate),
        //    new MySqlParameter("@P_pharmacyuuid", Guid.NewGuid()),
        //    new MySqlParameter("@P_addressLine1", pharmacyRequestModel.AddressLine1),
        //    new MySqlParameter("@P_addressLine2", pharmacyRequestModel.AddressLine2),
        //    new MySqlParameter("@P_city", pharmacyRequestModel.City),
        //    new MySqlParameter("@P_email", pharmacyRequestModel.Email),
        //    new MySqlParameter("@P_fax", pharmacyRequestModel.Fax),
        //    new MySqlParameter("@P_federalTaxIdNumber", pharmacyRequestModel.FederalTaxIdNumber),
        //    new MySqlParameter("@P_dataOrigin", pharmacyRequestModel.DataOrigin),
        //    new MySqlParameter("@P_deaExpirationDate", pharmacyRequestModel.DeaExpirationDate),
        //    new MySqlParameter("@P_medicareProviderId", pharmacyRequestModel.MedicareProviderId),
        //    new MySqlParameter("@P_storeNumber", pharmacyRequestModel.StoreNumber),
        //    new MySqlParameter("@P_status", pharmacyRequestModel.Status),
        //    new MySqlParameter("@P_pmsSystem", pharmacyRequestModel.PmsSystem),
        //    new MySqlParameter("@P_notes", pharmacyRequestModel.Notes),
        //    new MySqlParameter("@P_gpo", pharmacyRequestModel.Gpo),
        //    new MySqlParameter("@P_stateLicenseNumber", pharmacyRequestModel.StateLicenseNumber),
        //    new MySqlParameter("@P_stateLicenseExpiryDate", pharmacyRequestModel.StateLicenseExpiryDate),
        //    new MySqlParameter("@P_storeHours", pharmacyRequestModel.StoreHours),
        //    new MySqlParameter("@P_deliveryDriver", pharmacyRequestModel.DeliveryDriver),
        //    new MySqlParameter("@P_deliveryBoundary", pharmacyRequestModel.DeliveryBoundary),
        //    new MySqlParameter("@P_deliverySchedule", pharmacyRequestModel.DeliverySchedule),
        //    new MySqlParameter("@P_locationNotes", pharmacyRequestModel.LocationNotes),
        //    new MySqlParameter("@P_shipRxOrdersForPatientsAcrossYourState", pharmacyRequestModel.ShipRxOrdersForPatientsAcrossYourState),
        //    new MySqlParameter("@P_preferredShippingCarrier", pharmacyRequestModel.PreferredShippingCarrier),
        //    new MySqlParameter("@P_holidaysClosed", pharmacyRequestModel.HolidaysClosed),
        //    new MySqlParameter("@P_alternateHours", pharmacyRequestModel.AlternateHours),
        //    new MySqlParameter("@P_additionalHolidaysClosed", pharmacyRequestModel.AdditionalHolidaysClosed),
        //    new MySqlParameter("@P_timezone", pharmacyRequestModel.Timezone),
        //    new MySqlParameter("@P_pharmacyType", pharmacyRequestModel.PharmacyType),
        //    new MySqlParameter("@P_phoneNumber", pharmacyRequestModel.PhoneNumber),
        //    new MySqlParameter("@P_pharmacyEmployee", pharmacyRequestModel.PharmacyEmployee),
        //    new MySqlParameter("@P_servicesUnderStateMedicaid", pharmacyRequestModel.ServicesUnderStateMedicaid),
        //    new MySqlParameter("@P_servicesUnderMedicarePartB", pharmacyRequestModel.ServicesUnderMedicarePartB),
        //    new MySqlParameter("@P_isWorkersCompensationProgram", pharmacyRequestModel.IsWorkersCompensationProgram),
        //    new MySqlParameter("@P_additionalLicenses", pharmacyRequestModel.AdditionalLicenses),
        //    new MySqlParameter("@P_services", pharmacyRequestModel.Services),
        //    new MySqlParameter("@P_accreditations", pharmacyRequestModel.Accreditations),
        //    new MySqlParameter("@P_psao", pharmacyRequestModel.PSAO),
        //    new MySqlParameter("@P_contractRestrictionDetails", pharmacyRequestModel.ContractRestrictionDetails),
        //    new MySqlParameter("@P_wholeSalersInfo", pharmacyRequestModel.WholeSalersInfo),
        //    new MySqlParameter("@P_reportingToIQVIA", pharmacyRequestModel.ReportingToIQVIA),
        //    new MySqlParameter("@P_nationalProviderId", pharmacyRequestModel.NationalProviderId),
        //    new MySqlParameter("@P_ncpdp", pharmacyRequestModel.NCPDP),
        //    new MySqlParameter("@P_dea", pharmacyRequestModel.DEA),
        //    new MySqlParameter("@P_pharmacyName", pharmacyRequestModel.PharmacyName),
        //    new MySqlParameter("@P_legalBusinessName", pharmacyRequestModel.LegalBusinessName),
        //    new MySqlParameter("@P_pmsOutBoundTransferMethod", pharmacyRequestModel.PmsOutBoundTransferMethod),
        //    new MySqlParameter("@P_state", pharmacyRequestModel.State),
        //    new MySqlParameter("@P_longitude", pharmacyRequestModel.Longitude),
        //    new MySqlParameter("@P_latitude", pharmacyRequestModel.Latitude)
        //};

        //        // Make the call to the stored procedure and get the result
        //        var result = await _repository.GetAsync<PharmacyCreationResponse>(
        //            "CALL CreatePharmacy(@P_CreatedDate, @P_pharmacyuuid, @P_addressLine1, @P_addressLine2, @P_city, @P_email, @P_fax, @P_federalTaxIdNumber, @P_dataOrigin, @P_deaExpirationDate, @P_medicareProviderId, @P_storeNumber, @P_status, @P_pmsSystem, @P_notes, @P_gpo, @P_stateLicenseNumber, @P_stateLicenseExpiryDate, @P_storeHours, @P_deliveryDriver, @P_deliveryBoundary, @P_deliverySchedule, @P_locationNotes, @P_shipRxOrdersForPatientsAcrossYourState, @P_preferredShippingCarrier, @P_holidaysClosed, @P_alternateHours, @P_additionalHolidaysClosed, @P_timezone, @P_pharmacyType, @P_phoneNumber, @P_pharmacyEmployee, @P_servicesUnderStateMedicaid, @P_servicesUnderMedicarePartB, @P_isWorkersCompensationProgram, @P_additionalLicenses, @P_services, @P_accreditations, @P_psao, @P_contractRestrictionDetails, @P_wholeSalersInfo, @P_reportingToIQVIA, @P_nationalProviderId, @P_ncpdp, @P_dea, @P_pharmacyName, @P_legalBusinessName, @P_pmsOutBoundTransferMethod, @P_state, @P_longitude, @P_latitude)",
        //            parameters
        //        );

        //       return result;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        public async Task<PharmacyInstance> GetPharmaciesByIdentifiers(string? nationalProviderId, string? ncpdp, string? medicareProviderId)
        {
            try
            {
                var result = await _repository.GetAsync<PharmacyInstance>
                ("EXEC GetPharmaciesByIdentifiers @p_NationalProviderIdParam, @P_NCPDPParam, @P_medicareProviderIdParam", _context,
                            SqlHelper.CheckNotNull("@p_NationalProviderIdParam", nationalProviderId),
                            SqlHelper.CheckNotNull("@P_NCPDPParam", ncpdp),
                            SqlHelper.CheckNotNull("@P_medicareProviderIdParam", medicareProviderId)
                );

                if (result == null)
                {
                    return null;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }


        //public async Task<PharmacyCreationResponse?> CreatePharmacyAsync(PharmacyRequestModel pharmacyRequestModel)
        //{
        //    try
        //    {
        //        // Get USERID from appsettings.json
        //        var userId = _configuration.GetValue<string>("ShippingAPI:USERID") ?? "";
        //        // Create the request object
        //        var request = new AddressValidateRequest
        //        {
        //            Revision = "1",
        //            Address = new AddressModel
        //            {
        //                ID = "0",
        //                Address1 = pharmacyRequestModel.PharmacyDetails.StreetAddress,
        //                Address2 = "",
        //                City = pharmacyRequestModel.PharmacyDetails.City,
        //                State = pharmacyRequestModel.PharmacyDetails.State,
        //                Zip5 = pharmacyRequestModel.PharmacyDetails.PostalCode,
        //                Zip4 = ""
        //            },
        //            USERID = userId
        //        };

        //        AddressValidateResponse addressValidateResponse = await ValidateAddress(request);
        //        PositionStackAddress coordinate = Activator.CreateInstance<PositionStackAddress>();
        //        if (addressValidateResponse.Address.Error is null) // Valid Address
        //        {
        //            coordinate = await GetCoordinates((pharmacyRequestModel!.PharmacyDetails!.StreetAddress ?? "") + " " + (pharmacyRequestModel!.PharmacyDetails!.City ?? "") + " " + (pharmacyRequestModel.PharmacyDetails.State ?? "") + " " + (pharmacyRequestModel.PharmacyDetails.PostalCode ?? ""));
        //        }
        //        else
        //        {
        //            return new PharmacyCreationResponse { Id = -2, Code = "Invalid Address" };
        //        }

        //        // Create the parameters for the stored procedure using the updated structure
        //        var parameters = new[]
        //        {
        //    CheckNotNullDateTime("@P_CreatedDate", DateTime.Now),
        //    new MySqlParameter("@P_pharmacyuuid", Guid.NewGuid()), // Always generated as a new Guid
        //    CheckNotNullString("@P_addressLine1", pharmacyRequestModel.PharmacyDetails.StreetAddress),
        //    CheckNotNullString("@P_addressLine2", ""),
        //    CheckNotNullString("@P_city", pharmacyRequestModel.PharmacyDetails.City),
        //    CheckNotNullString("@P_email", pharmacyRequestModel.PharmacyDetails.Email),
        //    CheckNotNullString("@P_fax", pharmacyRequestModel.PharmacyDetails.Fax),
        //    CheckNotNullString("@P_federalTaxIdNumber", pharmacyRequestModel.PharmacyDetails.FederalTaxId),
        //    CheckNotNullString("@P_dataOrigin", pharmacyRequestModel.PharmacyDetails.DataOrigin),
        //    CheckNotNullDateTime("@P_deaExpirationDate", pharmacyRequestModel.PharmacyDetails.DeaExpDate),
        //    CheckNotNullString("@P_medicareProviderId", pharmacyRequestModel.PharmacyDetails.MedicareProviderId),
        //    CheckNotNullString("@P_storeNumber", pharmacyRequestModel.PharmacyDetails.StoreNumber),
        //    new MySqlParameter("@P_status", true),
        //    CheckNotNullString("@P_pmsSystem", pharmacyRequestModel.PharmacyDetails.PmsSystem),
        //    CheckNotNullString("@P_notes", pharmacyRequestModel.PharmacyDetails.Notes),
        //    CheckNotNullString("@P_gpo", pharmacyRequestModel!.ServiceDetails!.Gpo.Count>0 ? string.Join(",", pharmacyRequestModel!.ServiceDetails!.Gpo):""),
        //    CheckNotNullString("@P_stateLicenseNumber", pharmacyRequestModel.PharmacyDetails.StateLicenseNumber),
        //    CheckNotNullDateTime("@P_stateLicenseExpiryDate", pharmacyRequestModel.PharmacyDetails.LicenseExpiryDate),
        //    CheckNotNullString("@P_storeHours", JsonConvert.SerializeObject(pharmacyRequestModel.StoreHours)),
        //    CheckNotNullString("@P_deliveryBoundary", pharmacyRequestModel.ServiceDetails.PharmacyDeliveryBoundary),
        //    CheckNotNullString("@P_locationNotes", pharmacyRequestModel.ServiceDetails.PharmacyLocationNotes),
        //    new MySqlParameter("@P_shipRxOrdersForPatientsAcrossYourState", pharmacyRequestModel.ServiceDetails.ShipRxOrders),
        //    CheckNotNullString("@P_preferredShippingCarrier", string.Join(",", pharmacyRequestModel.ServiceDetails.PreferredShippingCarrier.Select(s => s.Name))),
        //    CheckNotNullString("@P_alternateHours", pharmacyRequestModel.StoreHours.Details),
        //    CheckNotNullString("@P_additionalHolidaysClosed", pharmacyRequestModel.StoreHours.AdditionalHolidays),
        //    CheckNotNullString("@P_phoneNumber", pharmacyRequestModel.PharmacyDetails.PhoneNumber),
        //    CheckNotNullString("@P_pharmacyEmployee", JsonConvert.SerializeObject(pharmacyRequestModel.PharmacistInformation)),
        //    new MySqlParameter("@P_servicesUnderStateMedicaid", pharmacyRequestModel.ServiceDetails.MedicaidContracted),
        //    new MySqlParameter("@P_servicesUnderMedicarePartB", pharmacyRequestModel.ServiceDetails.MedicareContracted),
        //    new MySqlParameter("@P_isWorkersCompensationProgram", false),//pharmacyRequestModel.ServiceDetails.WorkersCompensationParticipant),
        //    CheckNotNullString("@P_additionalLicenses", JsonConvert.SerializeObject(pharmacyRequestModel.ServiceDetails.AdditionalLicenses)),
        //    CheckNotNullString("@P_accreditations", pharmacyRequestModel.ServiceDetails.PharmacyAccreditations.Count>0 ? string.Join(",", pharmacyRequestModel.ServiceDetails.PharmacyAccreditations):""),
        //    CheckNotNullString("@P_psao", pharmacyRequestModel.InsuranceContract.PsaoAffiliation),
        //    CheckNotNullString("@P_contractRestrictionDetails", pharmacyRequestModel.ServiceDetails.ContractRestrictionDetails),
        //    CheckNotNullString("@P_wholeSalersInfo", ""),//pharmacyRequestModel.InsuranceContract.WholesalersInfo),
        //    new MySqlParameter("@P_reportingToIQVIA", pharmacyRequestModel.InsuranceContract.ReportingtoIQVIA), // DataType string to array modification
        //    CheckNotNullString("@P_ncpdp", pharmacyRequestModel.PharmacyDetails.Ncpdp.ToString()),
        //    CheckNotNullString("@P_dea", pharmacyRequestModel.PharmacyDetails.DeaNumber),
        //    CheckNotNullString("@P_legalBusinessName", pharmacyRequestModel.PharmacyDetails.LegalBusinessName),
        //    CheckNotNullString("@P_pmsOutBoundTransferMethod", ""),//pharmacyRequestModel.PharmacyDetails.PmsOutboundMethod),
        //    CheckNotNullString("@P_state", pharmacyRequestModel.PharmacyDetails.State),
        //    CheckNotNullDecimal("@P_longitude", (decimal)coordinate.Longitude),
        //    CheckNotNullDecimal("@P_latitude", (decimal)coordinate.Latitude),
        //    CheckNotNullString("@P_NPI", pharmacyRequestModel.PharmacyDetails.Npi.ToString()),
        //    CheckNotNullString("@P_OtherPMSSystem", pharmacyRequestModel.PharmacyDetails.Otherpmssystem),
        //    new MySqlParameter("@P_IsOfferDelivery", pharmacyRequestModel.ServiceDetails.OfferDelivery),
        //    CheckNotNullString("@P_DeliveryDays", string.Join(",", pharmacyRequestModel.ServiceDetails.DeliveryDays.Where(d => d.Selected).Select(d => d.Name))),
        //    CheckNotNullString("@P_StartTime", pharmacyRequestModel.ServiceDetails.StartTime),
        //    CheckNotNullString("@P_EndTime", pharmacyRequestModel.ServiceDetails.EndTime),
        //    CheckNotNullString("@P_DeliveryServiceMethod", pharmacyRequestModel.ServiceDetails.DeliveryServiceMethod),
        //    new MySqlParameter("@P_UseExistingDeliveryService", pharmacyRequestModel.ServiceDetails.DeliveryServices),
        //    new MySqlParameter("@P_IsRetail", pharmacyRequestModel.ServiceDetails.ServicesOffered.Any(s => s.Selected && s.Name.ToLower() == "retail")),
        //    new MySqlParameter("@P_IsMailOrder", pharmacyRequestModel.ServiceDetails.ServicesOffered.Any(s => s.Selected && s.Name.ToLower() == "mail order")),
        //    new MySqlParameter("@P_IsCompounding", pharmacyRequestModel.ServiceDetails.ServicesOffered.Any(s => s.Selected && s.Name.ToLower() == "compounding")),
        //    CheckNotNullString("@P_LegalName", pharmacyRequestModel.PharmacyDetails.LegalName),
        //    CheckNotNullString("@P_PostalCode", pharmacyRequestModel.PharmacyDetails.PostalCode),
        //    CheckNotNullString("@p_pbmContracts", pharmacyRequestModel.InsuranceContract.PbmContracts),
        //    CheckNotNullString("@p_shippingRetailPbmContract", pharmacyRequestModel.InsuranceContract.ShippingRetailPbmContract),
        //    CheckNotNullString("@p_dispensingFeesTerms", pharmacyRequestModel.InsuranceContract.DispensingFeesTerms)
        //};

        //        // Make the call to the stored procedure and get the result
        //        var result = await _repository.GetAsync<PharmacyCreationResponse>(
        //            "CALL CreatePharmaciesinfo_stage(@P_CreatedDate, @P_pharmacyuuid, @P_addressLine1, @P_addressLine2, @P_city, @P_email, @P_fax, @P_federalTaxIdNumber, @P_dataOrigin, @P_deaExpirationDate, @P_medicareProviderId, @P_storeNumber, @P_status, @P_pmsSystem, @P_notes, @P_gpo, @P_stateLicenseNumber, @P_stateLicenseExpiryDate, @P_storeHours, @P_deliveryBoundary, @P_locationNotes, " +
        //            "@P_shipRxOrdersForPatientsAcrossYourState, @P_preferredShippingCarrier, @P_alternateHours, @P_additionalHolidaysClosed, @P_phoneNumber, @P_pharmacyEmployee, @P_servicesUnderStateMedicaid, @P_servicesUnderMedicarePartB, @P_isWorkersCompensationProgram, @P_additionalLicenses, @P_accreditations, @P_psao, @P_contractRestrictionDetails, @P_wholeSalersInfo, " +
        //            "@P_reportingToIQVIA, @P_ncpdp, @P_dea, @P_legalBusinessName, @P_pmsOutBoundTransferMethod, @P_state, @P_longitude, @P_latitude," +
        //            "@P_NPI, @P_OtherPMSSystem, @P_IsOfferDelivery, @P_DeliveryDays, @P_StartTime, @P_EndTime, @P_DeliveryServiceMethod, " +
        //            "@P_UseExistingDeliveryService,@P_IsRetail, @P_IsMailOrder, @P_IsCompounding, @P_LegalName, @P_PostalCode, @p_pbmContracts, @p_shippingRetailPbmContract, @p_dispensingFeesTerms)",
        //            parameters
        //        );

        //        return result;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public async Task<PharmacyCreationResponse?> CreatePharmacyAsync(PharmacyRequestModel pharmacyRequestModel)
        {
            try
            {
                // Get USERID from appsettings.json
                var userId = _configuration.GetValue<string>("ShippingAPI:USERID") ?? "";

                // Create the request object for address validation
                var request = new AddressValidateRequest
                {
                    Revision = "1",
                    Address = new AddressModel
                    {
                        ID = "0",
                        Address1 = pharmacyRequestModel.PharmacyDetails.StreetAddress,
                        Address2 = "",
                        City = pharmacyRequestModel.PharmacyDetails.City,
                        State = pharmacyRequestModel.PharmacyDetails.State.Code,
                        Zip5 = pharmacyRequestModel.PharmacyDetails.PostalCode,
                        Zip4 = ""
                    },
                    USERID = userId
                };

                AddressValidateResponse addressValidateResponse = await ValidateAddress(request);
                PositionStackAddress coordinate = Activator.CreateInstance<PositionStackAddress>();
                if (addressValidateResponse.Address.Error is null) // Valid Address
                {
                    coordinate = await GetCoordinates((pharmacyRequestModel!.PharmacyDetails!.StreetAddress ?? "") + " " + (pharmacyRequestModel!.PharmacyDetails!.City ?? "") + " " + (pharmacyRequestModel.PharmacyDetails.State.Code ?? "") + " " + (pharmacyRequestModel.PharmacyDetails.PostalCode ?? ""));
                }
                else
                {
                    return new PharmacyCreationResponse { Id = -2, Code = "Invalid Address" };
                }

                // Prepare the parameters in the exact order as in the stored procedure
                var parameters = new[]
                {
            SqlHelper.CheckNotNullString("@p_legalName", pharmacyRequestModel.PharmacyDetails.LegalName),
            SqlHelper.CheckNotNullString("@p_legalBusinessName", pharmacyRequestModel.PharmacyDetails.LegalBusinessName),
            SqlHelper.CheckNotNullLong("@p_ncpdp", pharmacyRequestModel.PharmacyDetails.Ncpdp),
            SqlHelper.CheckNotNullLong("@p_npi", pharmacyRequestModel.PharmacyDetails.Npi),
            SqlHelper.CheckNotNullString("@p_deaNumber", pharmacyRequestModel.PharmacyDetails.DeaNumber),
            SqlHelper.CheckNotNullDateTime("@p_deaExpDate", pharmacyRequestModel.PharmacyDetails.DeaExpDate),
            SqlHelper.CheckNotNullString("@p_stateLicenseNumber", pharmacyRequestModel.PharmacyDetails.StateLicenseNumber),
            SqlHelper.CheckNotNullDateTime("@p_stateLicenseExpiryDate", pharmacyRequestModel.PharmacyDetails.LicenseExpiryDate),
            SqlHelper.CheckNotNullString("@p_medicareProviderId", pharmacyRequestModel.PharmacyDetails.MedicareProviderId),
            SqlHelper.CheckNotNullString("@p_federalTaxId", pharmacyRequestModel.PharmacyDetails.FederalTaxId),
            SqlHelper.CheckNotNullString("@p_storeNumber", pharmacyRequestModel.PharmacyDetails.StoreNumber),
            SqlHelper.CheckNotNullString("@p_dataOrigin", pharmacyRequestModel.PharmacyDetails.DataOrigin),
            SqlHelper.CheckNotNullString("@p_pmsSystem", pharmacyRequestModel.PharmacyDetails.PmsSystem!=null?pharmacyRequestModel.PharmacyDetails.PmsSystem.Name:string.Empty),
            SqlHelper.CheckNotNullString("@p_otherPmsSystem", pharmacyRequestModel.PharmacyDetails.Otherpmssystem),
            SqlHelper.CheckNotNullString("@p_streetAddress", pharmacyRequestModel.PharmacyDetails.StreetAddress),
            SqlHelper.CheckNotNullString("@p_city", pharmacyRequestModel.PharmacyDetails.City),
            SqlHelper.CheckNotNullString("@p_stateName", pharmacyRequestModel.PharmacyDetails.State.Name),
            SqlHelper.CheckNotNullString("@p_postalCode", pharmacyRequestModel.PharmacyDetails.PostalCode),
            SqlHelper.CheckNotNullString("@p_email", pharmacyRequestModel.PharmacyDetails.Email),
            SqlHelper.CheckNotNullString("@p_phoneNumber", pharmacyRequestModel.PharmacyDetails.PhoneNumber),
            SqlHelper.CheckNotNullString("@p_fax", pharmacyRequestModel.PharmacyDetails.Fax),
            SqlHelper.CheckNotNullString("@p_notes", pharmacyRequestModel.PharmacyDetails.Notes),
            SqlHelper.CheckNotNullString("@p_pharmacistInfo", XmlHelper.ObjectToXmlWithoutDeclaration(pharmacyRequestModel.PharmacistInformation)),
            SqlHelper.CheckNotNullString("@p_storehours", XmlHelper.ObjectToXmlWithoutDeclaration(pharmacyRequestModel.StoreHours)),
            SqlHelper.CheckNotNullString("@p_servicesOffered", string.Join(",", pharmacyRequestModel.ServiceDetails.ServicesOffered.Select(s => s.Name))),
            SqlHelper.CheckNotNullBool("@p_offerDelivery", pharmacyRequestModel.ServiceDetails?.OfferDelivery?.Key == "Y" ? true : false),
            SqlHelper.CheckNotNullString("@p_deliveryDays", pharmacyRequestModel.ServiceDetails?.DeliveryDays?.Count>0? string.Join(",", pharmacyRequestModel.ServiceDetails.DeliveryDays.Select(d => d.Name)):""),
            SqlHelper.CheckNotNullTimeSpan("@p_startTime", pharmacyRequestModel.ServiceDetails?.StartTime),
            SqlHelper.CheckNotNullTimeSpan("@p_endTime", pharmacyRequestModel.ServiceDetails?.EndTime),
            SqlHelper.CheckNotNullInt("@p_pharmacyDeliveryBoundary", pharmacyRequestModel.ServiceDetails?.PharmacyDeliveryBoundary),
            SqlHelper.CheckNotNullString("@p_deliveryServiceMethod", pharmacyRequestModel.ServiceDetails?.DeliveryServiceMethod?.Name),
            SqlHelper.CheckNotNullString("@p_carrierName",pharmacyRequestModel.ServiceDetails?.CarrierName),
            SqlHelper.CheckNotNullBool("@p_deliveryServices", pharmacyRequestModel.ServiceDetails?.DeliveryServices?.Key == "Y" ? true : false),
            SqlHelper.CheckNotNullString("@p_pharmacyLocationNotes", pharmacyRequestModel.ServiceDetails?.PharmacyLocationNotes),
            SqlHelper.CheckNotNullBool("@p_shipRxOrders", pharmacyRequestModel.ServiceDetails?.ShipRxOrders?.Key == "Y" ? true : false),
            SqlHelper.CheckNotNullString("@p_preferredShippingCarrier",pharmacyRequestModel.ServiceDetails?.PreferredShippingCarrier?.Count>0 ? string.Join(",", pharmacyRequestModel.ServiceDetails.PreferredShippingCarrier.Select(s => s.Name)):""),
            SqlHelper.CheckNotNullBool("@p_workersCompensation", pharmacyRequestModel.ServiceDetails?.WorkersCompensation.Key=="Y"? true: false),
            SqlHelper.CheckNotNullBool("@p_medicaidContracted", pharmacyRequestModel.ServiceDetails?.MedicaidContracted.Key=="Y"? true: false),
            SqlHelper.CheckNotNullBool("@p_medicareContracted", pharmacyRequestModel.ServiceDetails?.MedicareContracted.Key == "Y" ? true : false),
            SqlHelper.CheckNotNullString("@p_contractRestrictionDetails", pharmacyRequestModel.ServiceDetails?.ContractRestrictionDetails),
            SqlHelper.CheckNotNullString("@p_pharmacyAccreditations", pharmacyRequestModel.ServiceDetails?.PharmacyAccreditations.Count>0 ? string.Join(",", string.Join(",", pharmacyRequestModel.ServiceDetails.PharmacyAccreditations.Select(s => s.Name))):""),
            SqlHelper.CheckNotNullString("@p_gpo", pharmacyRequestModel.ServiceDetails?.Gpo?.Count > 0 ? string.Join(",", pharmacyRequestModel.ServiceDetails.Gpo.Select(s => s.Name)) : ""),
            SqlHelper.CheckNotNullString("@p_additionalLicenses",pharmacyRequestModel.ServiceDetails?.AdditionalLicenses?.Count>0? XmlHelper.ObjectToXmlWithoutDeclaration(pharmacyRequestModel.ServiceDetails.AdditionalLicenses):null),
            SqlHelper.CheckNotNullBool("@p_retailContract", pharmacyRequestModel.InsuranceContract?.RetailContract?.Key == "Y" ? true : false),
            SqlHelper.CheckNotNullString("@p_insurancePlans", pharmacyRequestModel.InsuranceContract?.InsurancePlans?.Count>0 ? string.Join(",", string.Join(",", pharmacyRequestModel.InsuranceContract.InsurancePlans.Select(s => s.Name))):""),
            SqlHelper.CheckNotNullString("@p_otherInsurancePlan",pharmacyRequestModel.InsuranceContract?.otherInsurancePlan),           
            SqlHelper.CheckNotNullBool("@p_mailOrder", pharmacyRequestModel.InsuranceContract?.MailOrder?.Key == "Y" ? true : false),
            SqlHelper.CheckNotNullString("@p_mailOrderContract", pharmacyRequestModel.InsuranceContract?.MailOrderContract?.Count>0 ? string.Join(",", string.Join(",", pharmacyRequestModel.InsuranceContract.MailOrderContract.Select(s => s.Name))):""),
            SqlHelper.CheckNotNullString("@p_otherMailOrderContract",pharmacyRequestModel.InsuranceContract?.OtherMailOrderContract),
            SqlHelper.CheckNotNullDateTime("@p_createdDate", DateTime.Now),
            SqlHelper.CheckNotNullDecimal("p_latitude", (decimal)coordinate.Latitude),
            SqlHelper.CheckNotNullDecimal("@p_longitude", (decimal)coordinate.Longitude),
            SqlHelper.CheckNotNullString("@JSONBody", JsonConvert.SerializeObject(pharmacyRequestModel))

        };

                // Execute the stored procedure
                var result = await _repository.GetAsync<PharmacyCreationResponse>(
                    "EXEC CreatePharmaciesInfoStage " +
                    "@p_legalName, @p_legalBusinessName, @p_ncpdp, @p_npi, @p_deaNumber, @p_deaExpDate, @p_stateLicenseNumber, @p_stateLicenseExpiryDate, @p_medicareProviderId, @p_federalTaxId, @p_storeNumber, @p_dataOrigin, @p_pmsSystem, @p_otherPmsSystem, @p_streetAddress, " +
                    "@p_city, @p_stateName, @p_postalCode, @p_email, @p_phoneNumber, @p_fax, @p_notes, @p_pharmacistInfo, @p_storehours, @p_servicesOffered, @p_offerDelivery, @p_deliveryDays, @p_startTime, @p_endTime, @p_pharmacyDeliveryBoundary, @p_deliveryServiceMethod, @p_carrierName, @p_deliveryServices, " +
                    "@p_pharmacyLocationNotes, @p_shipRxOrders, @p_preferredShippingCarrier, @p_workersCompensation, @p_medicaidContracted, @p_medicareContracted, @p_contractRestrictionDetails, @p_pharmacyAccreditations, @p_gpo, @p_additionalLicenses,@p_retailContract,@p_insurancePlans,@p_otherInsurancePlan,"+
                    "@p_localInsurance,@p_localInsurancePlans,@p_otherLocalInsurancePlans,@p_mailOrder,@p_mailOrderContract,@p_otherMailOrderContract, @p_createdDate,@p_latitude,@p_longitude,@JSONBody ",
                    _context,
                    parameters
                );
                if (result?.Id > 0)
                {
                    var requestBody = new
                    {
                        data = JsonConvert.SerializeObject(new { PharmacyInfoId = result.Id })
                    };
                    await _stackyonService.CallServiceApi(requestBody);
                }

                return result;

            }
            catch (Exception ex)
            {
                // Handle any exceptions as needed
                throw new Exception("An error occurred while creating the pharmacy.", ex);
            }
        }
        

        private async Task<AddressValidateResponse> ValidateAddress(AddressValidateRequest request)
        {
            // Generate XML from the object
            var xmlContent = XmlHelper.ObjectToXml(request);
            string baseUrl = _configuration.GetValue<string>("ShippingAPI:BaseUrl") ?? "";
            // Create the full API URL
            var apiUrl = $"{baseUrl}ShippingAPI.dll?API=Verify&XML={xmlContent}";
            var response = await _httpClientHelper.GetRequestAsync(apiUrl);
            return XmlHelper.DeserializeXml<AddressValidateResponse>(response);
        }
        
        private async Task<PositionStackAddress> GetCoordinates(string address)
        {
            var accessKey = _configuration.GetValue<string>("PositionStackAPI:accessKey") ?? "";
            string baseUrl = _configuration.GetValue<string>("PositionStackAPI:BaseUrl") ?? "";
            var url = $"{baseUrl}v1/forward?access_key={accessKey}&query={address}";

            var response = await _httpClientHelper.SendRequestAsync<PositionStackResponse>(HttpMethod.Get, url);

            return response.Data.Count() > 0 ? response.Data[0] : Activator.CreateInstance<PositionStackAddress>();
        }
        public async Task PostMethodsCall()
        {
            var url = "https://api.example.com/endpoint";
            var response = await _httpClientHelper.SendRequestAsync<string>(HttpMethod.Get, url);
            //var response = await _httpClientHelper.SendRequestAsync<MyDataModel>(HttpMethod.Post, url, data);

            var posturl = "https://api.example.com/endpoint"; // Replace with your actual URL
            var data = new { };
            var postresponse = await _httpClientHelper.SendRequestAsync<string>(HttpMethod.Post, url, data); // Post with data


        }

        public async Task<List<ConfigurationType>> GetConfigureNamesByType(string type)
        {
            try
            {
                var result = await _repository.GetAllAsync<ConfigurationType>
                ("EXEC GetConfigurationNamesByType @Type", _context,
                            SqlHelper.CheckNotNull("@Type", type)
                );

                if (result == null)
                {
                    return null;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }


        public async Task<List<Prescription>> GetPharmacyPrescriptionsByStatus(string status, string pharmacyInfoId)
        {
            try
            {
                var result = await _repository.GetAllAsync<Prescription>
                ("EXEC GetPharmacyPrescriptionsByStatus @p_pharmacyInfoId, @p_status", _context,
                            SqlHelper.CheckNotNull("@p_pharmacyInfoId", pharmacyInfoId),
                            SqlHelper.CheckNotNull("@p_status", status)
                );

                if (result == null)
                {
                    return null;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UpdatePharmacyPrescriptionStatus(string status, string Id,string updatedBy, string rxPharmacyInfoId, string encodedUrl)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                 {
                    new SqlParameter("@Id", SqlDbType.Int) { Value = Id},
                    new SqlParameter("@status", SqlDbType.NVarChar, 50) { Value = status },
                    new SqlParameter("@updatedBy", SqlDbType.NVarChar, 50) { Value = updatedBy }
                 };
                var result = await _repository.ExecuteScalarAsync("EXEC UpdatePharmacyPrescriptionStatus @p_Id, @p_status,@p_updatedBy", _context,
                                     SqlHelper.CheckNotNull("@p_Id", Id),
                                     SqlHelper.CheckNotNull("@p_status", status),
                                     SqlHelper.CheckNotNull("@p_updatedBy", updatedBy));
                if (result == null)
                {
                   return 0;
                }
                if(result > 0)
                {
                    _ = Task.Run(async () =>
                        {
                            try
                            {
                                
                                var requestBody = new
                                {
                                    data = JsonConvert.SerializeObject(new Dictionary<string, object>
                                    {
                                        { "rxpharmacyInfo.RxPharmacyInfoId", Convert.ToInt32(rxPharmacyInfoId) },
                                        { "rxpharmacyInfo.ProcessStatus", (status =="2" ? 102  : (status =="100" ? 200 :201)) }
                                    }),
                                    encodedUrl = encodedUrl ?? "",
                                };

                                await _stackyonService.CallServiceApi(requestBody);
                            }
                            catch (Exception ex)
                            {
                                // Log the exception or handle it as needed
                                Console.WriteLine($"Error in async execution: {ex.Message}");
                            }
                        });
                }
                
                return Convert.ToInt32(result);
            }
            catch
            {
                throw;
            }
        }


        public async Task<PrescriptionStausCount> GetPharmacyPrescriptionsStatusCounts(string pharmacyId)
        {
            try
            {
                var result = await _repository.GetAsync<PrescriptionStausCount>
                ("EXEC GetPharmacyPrescriptionsStatusCounts @p_pharmacyId", _context,
                            SqlHelper.CheckNotNull("@p_pharmacyId", pharmacyId)
                );

                if (result == null)
                {
                    return null;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<UserProfile> GetUserProfile(string email)
        {
            try
            {
                var result = await _repository.GetAsync<UserProfile>("EXEC GetUserProfileByEmail @p_email", _context,
                                SqlHelper.CheckNotNull("@p_email", email));

                if (result == null)
                {
                    return null;
                }
                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<PositionStackAddress> ValidateAddressAsync(AddressModel addressModel)
        {
            try
            {

                var userId = _configuration.GetValue<string>("ShippingAPI:USERID") ?? "";
                addressModel.ID = "0";
                // Create the request object for address validation
                var request = new AddressValidateRequest
                {
                    Revision = "1",
                    Address = addressModel,
                    USERID = userId
                };

                AddressValidateResponse addressValidateResponse = await ValidateAddress(request);
                PositionStackAddress coordinate = Activator.CreateInstance<PositionStackAddress>();
                if (addressValidateResponse.Address.Error is null) // Valid Address
                {
                    return await GetCoordinates((addressModel?.Address1 ?? "") + " " + (addressModel!.City ?? "") + " " + (addressModel?.State ?? "") + " " + (addressModel?.Zip5 ?? ""));
                }
                else
                {
                    return coordinate;
                }
            }
            catch
            {
                throw;
            }

        }
        public async Task<PharmacyCreationResponse> Createorupdatepharmacy([FromBody] PharmacyInfo pharmacyInfo)
        {
            try
            {
                
                // Prepare the parameters in the exact order as in the stored procedure
                var parameters = new[]
                {
                    SqlHelper.CheckNotNullString("@LegalName", pharmacyInfo.LegalName),
                    SqlHelper.CheckNotNullLong("@Npi", pharmacyInfo.Npi),
                    SqlHelper.CheckNotNullLong("@Ncpdp", pharmacyInfo.Ncpdp),
                    
                    SqlHelper.CheckNotNullString("@DeaNumber", pharmacyInfo.DeaNumber),
                    SqlHelper.CheckNotNullString("@StreetAddress", pharmacyInfo.StreetAddress),
                    SqlHelper.CheckNotNullString("@City", pharmacyInfo.City),
                    SqlHelper.CheckNotNullString("@State", pharmacyInfo.State),
                    SqlHelper.CheckNotNullString("@PostalCode", pharmacyInfo.PostalCode),
                    SqlHelper.CheckNotNullString("@PhoneNumber", pharmacyInfo.PhoneNumber),
                    SqlHelper.CheckNotNullString("@Fax", pharmacyInfo.Fax),

                    SqlHelper.CheckNotNullString("@JSONBody", JsonConvert.SerializeObject(pharmacyInfo)),
                    SqlHelper.CheckNotNullString("@email", pharmacyInfo.email)

                };

                // Execute the stored procedure
                var result = await _repository.GetAsync<PharmacyCreationResponse>(
                    "EXEC CreatePharmacy " +
                    "@LegalName, @Npi,  @Ncpdp, @DeaNumber, @StreetAddress, @City, @State, @PostalCode, @PhoneNumber, @Fax, @JSONBody, @email ",
                    _Einsteincontext,
                    parameters
                );
                if (result?.Id > 0)
                {
                    var requestBody = new
                    {
                        data = JsonConvert.SerializeObject(new { PharmacyInfoId = result.Id })
                    };
                    //await _stackyonService.CallServiceApi(requestBody);
                }

                return result;

            }
            catch (Exception ex)
            {
                // Handle any exceptions as needed
                throw new Exception("An error occurred while creating the pharmacy.", ex);
            }
        }
    }
}
