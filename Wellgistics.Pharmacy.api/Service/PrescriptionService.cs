using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Wellgistics.Pharmacy.api.Common;
using Wellgistics.Pharmacy.api.IRepositries;
using Wellgistics.Pharmacy.api.Models;
using Wellgistics.Pharmacy.api.IService;
using Wellgistics.Pharmacy.api.Repository;

namespace Wellgistics.Pharmacy.api.Service
{
    public class PrescriptionService : IPrescriptionService
    {

        private readonly IRepository _repository;
        private readonly HttpClientHelper _httpClientHelper;
        private readonly IConfiguration _configuration;
        private readonly IStackyonService _stackyonService;
        private readonly PharmacyDbContext _context;
        public PrescriptionService(IRepository repository, HttpClientHelper httpClientHelper, IConfiguration configuration, IStackyonService stackyonService, PharmacyDbContext context)
        {
            _repository = repository;
            _httpClientHelper = httpClientHelper;
            _configuration = configuration;
            _stackyonService = stackyonService;
            _context = context;
        }

        public async Task<PharmacyCreationResponse?> CreatePrescriptionAsync(PrescriptionRequestModel prescriptionModel, string encodedUrl)
        {
            try
            {
                // Get USERID from appsettings.json
                // var userId = _configuration.GetValue<string>("ShippingAPI:USERID") ?? "";

                // Prepare the parameters in the exact order as in the stored procedure
                var parameters = new[]
                {
                    SqlHelper.CheckNotNullString("@p_rxId", prescriptionModel.RxId),
                    SqlHelper.CheckNotNullString("@p_insurance", prescriptionModel.RxInsurance ?? "OptumRx"),
                    SqlHelper.CheckNotNullString("@p_drugName", prescriptionModel.RxDrugName),
                    SqlHelper.CheckNotNullLong("@p_daw", prescriptionModel.RxDAW),
                    SqlHelper.CheckNotNullString("@p_deliveryType", prescriptionModel.RxDeliveryMethod),
                    SqlHelper.CheckNotNullLong("@p_quantity", prescriptionModel.RxQuantity),
                     SqlHelper.CheckNotNullLong("@p_ncpdp", prescriptionModel.RxPharmacyNCPDP),
                    SqlHelper.CheckNotNullString("@p_status", "InQueue"),
                      SqlHelper.CheckNotNullString("@p_ndc", prescriptionModel.RxSourceNDC),
                    SqlHelper.CheckNotNullString("@p_JSONBody", JsonConvert.SerializeObject(prescriptionModel)),
                     SqlHelper.CheckNotNullString("@p_reqStatus", "Success"),
                        SqlHelper.CheckNotNullString("@p_createdBy", "Einstein Flow"),
                      SqlHelper.CheckNotNullString("@p_encodedUrl", encodedUrl),
                     SqlHelper.CheckNotNullInt("@rxTransactionId", prescriptionModel.RxTransactionId),
                      SqlHelper.CheckNotNullInt("@rxPharmacyInfoId", prescriptionModel.RxPharmacyInfoId),
                };

                // Execute the stored procedure
                var result = await _repository.GetAsync<PharmacyCreationResponse>(
                    "EXEC CreatePrescriptionInfo " +
                    "@p_rxId, @p_insurance, @p_drugName, @p_daw, @p_deliveryType, @p_quantity, @p_ncpdp, @p_status,@p_ndc, @p_JSONBody, @p_reqStatus,@p_createdBy,@p_encodedUrl,@rxTransactionId,@rxPharmacyInfoId",
                    _context,
                    parameters
                );
                return result;
            }
            catch (Exception ex)
            {
                // Handle any exceptions as needed
                throw new Exception("An error occurred while creating the prescription.", ex);
            }
        }
        public async Task<RxPharmacyOptions> GetrxPharmacyOptions(string rxTransactionId)
        {
            try
            {
                RxPharmacyOptions objrx = new RxPharmacyOptions();
                var result = await _repository.GetAllAsync<RxPharmacy>
                ("EXEC GetrxPharmacyData @p_rxTransactionId", _context,
                            SqlHelper.CheckNotNull("@p_rxTransactionId", rxTransactionId)
                );

                if (result != null)
                {
                    objrx.pharmacies = result;
                }

                var result1 = await _repository.GetAllAsync<RxOptions>
                ("EXEC GetrxPatientPrescriptionOptions @p_rxTransactionId", _context,
                            SqlHelper.CheckNotNull("@p_rxTransactionId", rxTransactionId)
                );
                if (result1 != null)
                {
                    objrx.Options = result1;
                }

                return objrx;
            }
            catch
            {
                throw;
            }
        }
        public async Task<PharmacyCreationResponse?> CreatePrescriptionsAsync(List<PrescriptionRequestModel> prescriptionModel, string encodedUrl)
        {
            try
            {
                // Get USERID from appsettings.json
                // var userId = _configuration.GetValue<string>("ShippingAPI:USERID") ?? "";

                // Prepare the parameters in the exact order as in the stored procedure
                var parameters = new[]
                {
                    SqlHelper.CheckNotNullString("@prescriptionXml", XmlHelper.ObjectToXmlWithoutDeclaration(prescriptionModel)),
                    SqlHelper.CheckNotNullString("@encodedUrl", encodedUrl),
                    SqlHelper.CheckNotNullString("@JSONBody", JsonConvert.SerializeObject(prescriptionModel))


                };

                // Execute the stored procedure
                var result = await _repository.GetAsync<PharmacyCreationResponse>(
                    "EXEC CreatePrescriptionsInfo " +
                    "@prescriptionXml, @encodedUrl, @JSONBody",
                    _context,
                    parameters
                );
                return result;
            }
            catch (Exception ex)
            {
                // Handle any exceptions as needed
                throw new Exception("An error occurred while creating the prescription.", ex);
            }
        }

    }
}
