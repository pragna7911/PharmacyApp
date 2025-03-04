using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Wellgistics.Pharmacy.api.IService;
using Wellgistics.Pharmacy.api.Models;

namespace Wellgistics.Pharmacy.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {

        private readonly IPrescriptionService _prescriptionService;
        private readonly IStackyonService _istackyonService;
        private readonly ILogger<PharmacyController> _logger;
        public PrescriptionController(IPrescriptionService prescriptionService, IStackyonService istackyonService, ILogger<PharmacyController> logger)
        {
            _prescriptionService = prescriptionService;
            _istackyonService = istackyonService;
            _logger = logger;
        }


        [HttpPost("createPrescription")]
        [Authorize(Policy = "ApiKeyPolicy")]
        public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionRequestModel prescriptionmodel, string encodedUrl)
        {
            try
            {
                if (prescriptionmodel == null)
                {
                    return BadRequest("Invalid data.");
                }
                var result = await _prescriptionService.CreatePrescriptionAsync(prescriptionmodel, encodedUrl);
                //if( result!.Id ==-1) throw new ArgumentException("Argument exception");
                if (result!.Id > 0)
                {
                    return Ok(new { Code = 200, Message = "Prescription created successfully" });
                }
                else
                {
                    return result!.Id is not null ? Ok(new { Result = result }) : Ok(new { Code = 500, Message = "An error occurred while creating the prescription." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return Ok(new { Code = 500, Mesage = ex.Message });
            }

        }

        [HttpGet("GetrxPharmacyOptions")]
        public async Task<IActionResult> GetrxPharmacyOptions(string rxTransactionId)
        {
            try
            {
                var result = await _prescriptionService.GetrxPharmacyOptions(rxTransactionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("UpdateRxPharmacyPrescription")]
        public async Task<IActionResult> UpdateRxPharmacyPrescription([FromBody] RxResponse rxResponseModel, string encodedUrl)
        {
            try
            {
                var objd = new Dictionary<string, object>();
                if ((rxResponseModel?.RxPharmacyInfoId is null || rxResponseModel.RxPharmacyInfoId == 0) && (rxResponseModel?.PrescriptionOptionId is null || rxResponseModel.PrescriptionOptionId == 0))
                {
                    return BadRequest("RxPharmacyInfoId or PrescriptionOptionId required.");
                }
                if (rxResponseModel.RxPharmacyInfoId > 0)
                {
                    objd.Add("rxPharmacyInfo.RxPharmacyInfoId", rxResponseModel.RxPharmacyInfoId);
                    objd.Add("rxPharmacyInfo.RxTransactionId", Convert.ToInt32(rxResponseModel.RxTransactionId));
                    objd.Add("rxPharmacyInfo.IsSelected", rxResponseModel.IsSelected ? 1 : 0);

                    if (rxResponseModel.IsDefaultPharmacy)
                    {

                        objd.Add("patientInfo.PatientInfoId", rxResponseModel.PatientInfoId);
                        objd.Add("patientInfo.PreferedPharmacyId", rxResponseModel.PharmacyInfoId);
                        objd.Add("patientInfo.PatientLastName", rxResponseModel.PatientLastName);
                        objd.Add("patientInfo.PatientDOB", rxResponseModel.PatientDOB);
                        objd.Add("patientInfo.PatientPhone", rxResponseModel.PatientPhone);
                    }
                }
                 if (rxResponseModel.PrescriptionOptionId > 0)
                {
                    objd.Add("patientprescriptionoptions.PrescriptionOptionId", rxResponseModel.PrescriptionOptionId);
                    objd.Add("patientprescriptionoptions.RxTransactionId", Convert.ToInt32(rxResponseModel.RxTransactionId));
                    objd.Add("patientprescriptionoptions.IsSelected", rxResponseModel.IsSelected ? 1 : 0);
                    objd.Add("patientprescriptionoptions.PatientInfoId", rxResponseModel.PatientInfoId);
                }


                var requestBody = new
                {
                    data = JsonConvert.SerializeObject(objd),
                    encodedUrl = encodedUrl ?? "",
                };

                await _istackyonService.CallServiceApi(requestBody);
                //await _pharmacyService.UpdatePharmacyPrescriptionStatus(status, Id, updatedBy);

                return Ok(new { Code = 200, Message = "successfully Added" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("createPrescriptions")]
        [Authorize(Policy = "ApiKeyPolicy")]
        public async Task<IActionResult> CreatePrescriptions([FromBody] List<PrescriptionRequestModel> prescriptionmodel, string encodedUrl)
        {
            try
            {
                if (prescriptionmodel == null)
                {
                    return BadRequest("Invalid data.");
                }
                var result = await _prescriptionService.CreatePrescriptionsAsync(prescriptionmodel, encodedUrl);
                //if( result!.Id ==-1) throw new ArgumentException("Argument exception");
                if (result!.Id > 0)
                {
                    return Ok(new { Code = 200, Message = "Prescription created successfully" });
                }
                else
                {
                    return result!.Id is not null ? Ok(new { Result = result }) : StatusCode(500,new { Code = 500, Message = "An error occurred while creating the prescription." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return Ok(new { Code = 500, Mesage = ex.Message });
            }

        }
    }
}
