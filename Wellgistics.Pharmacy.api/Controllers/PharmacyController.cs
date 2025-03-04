using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wellgistics.Pharmacy.api.IService;
using Wellgistics.Pharmacy.api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Wellgistics.Pharmacy.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly ILogger<PharmacyController> _logger;
        public PharmacyController(IPharmacyService pharmacyService, ILogger<PharmacyController> logger)
        {
            _pharmacyService = pharmacyService;
            _logger = logger;
        }
        /// <summary>
        /// CreatePharmacy
        /// </summary>
        /// <remarks>
        /// Sample Request: <br/>
        /// {<br/>
        ///   "RuleSetInfo": {<br/>
        ///     "RuleSetName": "Sample RuleSet",<br/>
        ///     "Status": "Active",<br/>
        ///     "CreateDateTime": "2024-12-20T10:00:00",<br/>
        ///     "CreatedBy": "Admin"<br/>
        ///   },<br/>
        ///     "ParameterName": "Sample Parameter",<br/>
        ///     "ParameterType": "String",<br/>
        ///     "Priority": "High"<br/>
        ///   }<br/>
        /// }
        /// </remarks>
        /// <param name="ruleSetRequestModel"></param>
        /// <returns>Created the RuleSet Data</returns>
        /// <response code="200">Return the Status as 200 When Rule Set With Parmas Created</response>
        /// <response code="400">Return the Status as 400 When Invalid request body</response>
        /// <response code="500">Incase of exception</response>
        [HttpPost("createPharmacy")]
        public async Task<IActionResult> CreatePharmacy([FromBody] PharmacyRequestModel pharmacyRequestModel)
        {
            try
            {
                if (pharmacyRequestModel == null)
                {
                    return BadRequest("Invalid data.");
                }
                var result = await _pharmacyService.CreatePharmacyAsync(pharmacyRequestModel);
                //if( result!.Id ==-1) throw new ArgumentException("Argument exception");
                if (result!.Id > 0)
                {
                    return Ok(new { Message = "Pharmacy created successfully", Result = result });
                }
                else
                {
                    return result!.Id is not null ? Ok(new { Result = result }) : StatusCode(500, "An error occurred while creating the RuleSet.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet("PharmacyByIdentifiers")]
        public async Task<IActionResult> GetPharmacyByIdentifiers(string? nationalProviderId, string? ncpdp, string? medicareProviderId)
        {
            try
            {
                var result = await _pharmacyService.GetPharmaciesByIdentifiers(nationalProviderId, ncpdp, medicareProviderId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("ConfigureNamesByType")]
        public async Task<IActionResult> GetConfigureNamesByType(string type)
        {
            try
            {
                var result = await _pharmacyService.GetConfigureNamesByType(type);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        // Get all pharmacies
        [HttpGet("pharmacies")]
        [Authorize(Policy = "Auth0JwtPolicy")]
        public ActionResult GetPharmacies()
        {
           var data =  new
            {
                inQueueCount = 11,
                expiringCount = 5,
                totalQueueCount = 11,
                SLACount = 3,
                TotalSLA = 7,
                inprocessCount = 6,
                completedCount = 7,
                completedPercentage = 27.42,
                rejectedCount = 8,
                rejectedPercentage = 14.42
            };
            // Sample data for pharmacies
            //List<object> Pharmacies = new List<object>
            //{
            //    new
            //    {
            //        id = 1,
            //        name = "WellCare Pharmacy",
            //        address = "123 Pharmacy St., Wellness City, 12345",
            //        contact = "123-456-7890",
            //        availableMedications = new[]
            //        {
            //            new { name = "Aspirin", quantity = 50, price = 10 },
            //            new { name = "Ibuprofen", quantity = 30, price = 15 },
            //            new { name = "Amoxicillin", quantity = 20, price = 20 }
            //        },
            //        insuranceCoverage = new[]
            //        {
            //            new { provider = "HealthCare Insurance", plan = "Basic Health Plan", coverage = "Covers up to 50% for all medications." },
            //            new { provider = "MediCare", plan = "MediCare Plus", coverage = "Covers up to 80% for prescriptions." }
            //        }
            //    },
            //    new
            //    {
            //        id = 2,
            //        name = "City Pharmacy",
            //        address = "456 City Blvd., Citytown, 67890",
            //        contact = "987-654-3210",
            //        availableMedications = new[]
            //        {
            //            new { name = "Paracetamol", quantity = 100, price = 5 },
            //            new { name = "Metformin", quantity = 40, price = 12 },
            //            new { name = "Lipitor", quantity = 25, price = 30 }
            //        },
            //        insuranceCoverage = new[]
            //        {
            //            new { provider = "Blue Cross Insurance", plan = "Standard Health Plan", coverage = "Covers up to 70% for all medications." }
            //        }
            //    }
            //};
            //return StatusCode(500, "Internal Error");
            return Ok(data);
        }

        // Get all insurance providers
        [HttpGet("insurance-providers")]
        //[Authorize(Policy = "Auth0JwtPolicy")]
        public ActionResult<List<object>> GetInsuranceProviders()
        {
            List<object> InsuranceProviders = new List<object>
            {
                new
                {
                    id = 1,
                    name = "HealthCare Insurance",
                    planTypes = new[]
                    {
                        new { plan = "Basic Health Plan", coverage = "50% coverage on prescriptions" },
                        new { plan = "Premium Health Plan", coverage = "80% coverage on prescriptions" }
                    }
                },
                new
                {
                    id = 2,
                    name = "MediCare",
                    planTypes = new[]
                    {
                        new { plan = "MediCare Plus", coverage = "80% coverage on prescriptions" }
                    }
                },
                new
                {
                    id = 3,
                    name = "Blue Cross Insurance",
                    planTypes = new[]
                    {
                        new { plan = "Standard Health Plan", coverage = "70% coverage on prescriptions" }
                    }
                }
            };
            return Ok(InsuranceProviders);
        }

        [HttpGet("GetPharmacyPrescriptionsByStatus")]
        [Authorize(Policy = "Auth0JwtPolicy")]
        public async Task<IActionResult> GetPharmacyPrescriptionsByStatus(string status,string pharmacyInfoId)
        {
            try
            {
                var result = await _pharmacyService.GetPharmacyPrescriptionsByStatus(status,pharmacyInfoId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("UpdatePharmacyPrescriptionStatus")]
        public async Task<IActionResult> UpdatePharmacyPrescriptionStatus(string status, string Id,string updatedBy, string rxPharmacyInfoId, string encodedUrl)
        {
            try
            {
                var result = await _pharmacyService.UpdatePharmacyPrescriptionStatus(status, Id, updatedBy, rxPharmacyInfoId, encodedUrl);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetPharmacyPrescriptionsStatusCount")]
        //[Authorize(Policy = "Auth0JwtPolicy")]
        public async Task<IActionResult> GetPharmacyPrescriptionsStatusCount(string pharmacyId)
        {
            try
            {
                var result = await _pharmacyService.GetPharmacyPrescriptionsStatusCounts(pharmacyId);
                //string body = @"<h1>Hello Bhanu,</h1><p>Your account has been created successfully. Below are your details:</p>" +
                //    "<ul> <li><strong>Username:</strong>" + "Bhanu.panathala@amzur.com" + "</li>       <li><strong>Password:</strong>" + "Test@123" + "</li>   </ul>";


                //SendEmail("bhanu.panathala@amzur.com", "Your Account Details", body);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("UserProfile")]
        [Authorize(Policy = "Auth0JwtPolicy")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                string? emailClaim = HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == "email")?.Value;
                if (emailClaim == null)
                {
                    return BadRequest("Email claim not found.");
                }
                var result = await _pharmacyService.GetUserProfile(emailClaim?? "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("Createorupdatepharmacy")]
        [Authorize(Policy = "ApiKeyPolicy")]
        public async Task<IActionResult> Createorupdatepharmacy([FromBody] PharmacyInfo pharmacyInfo)
        {
            try
            {
                if (pharmacyInfo == null)
                {
                    return BadRequest("Invalid data.");
                }
                var result = await _pharmacyService.Createorupdatepharmacy(pharmacyInfo);
                //if( result!.Id ==-1) throw new ArgumentException("Argument exception");
                if (result!.Id > 0)
                {
                    return Ok(new { Message = "Pharmacy created successfully", Result = result });
                }
                else
                {
                    return result!.Id is not null ? Ok(new { Result = result }) : StatusCode(500, "An error occurred while creating the RuleSet.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }

        }
        [HttpPost("ValidateAddressAsync")]
        public async Task<IActionResult> ValidateAddressAsync([FromBody] AddressModel addressModel)
        {
            try
            {
                if (addressModel == null)
                {
                    return BadRequest("Invalid data.");
                }
                var result = await _pharmacyService.ValidateAddressAsync(addressModel);
                //if( result!.Id ==-1) throw new ArgumentException("Argument exception");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error {Message}", ex.Message);
                return StatusCode(500, ex.Message);
            }

        }


    }

}
