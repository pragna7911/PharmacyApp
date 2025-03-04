using Azure;
using Wellgistics.Pharmacy.api.Common;
using Wellgistics.Pharmacy.api.IRepositries;
using Wellgistics.Pharmacy.api.IService;
using Wellgistics.Pharmacy.api.Models;
using Wellgistics.Pharmacy.api.Repository;

namespace Wellgistics.Pharmacy.api.Service
{
    public class ServiceLogService : IServiceLogService
    {
        private readonly IRepository _repository;
        private readonly PharmacyDbContext _context;
        private readonly ILogger<ServiceLogService> _logger;
        private readonly IConfiguration _config;
        public ServiceLogService(IRepository repository, PharmacyDbContext pharmacyDbContext, ILogger<ServiceLogService> logger, IConfiguration config)
        {
            _repository = repository;
            _context = pharmacyDbContext;
            _logger = logger;
            _config = config;
        }
        public async Task LogServiceCallAsync(ServiceLogModel serviceLog)
        {
            try
            {
                var parameters = new[]
            {
                SqlHelper.CheckNotNullString("@SourceServiceName", serviceLog.SourceServiceName),
                SqlHelper.CheckNotNullString("@DestinationService", serviceLog.DestinationService),
                SqlHelper.CheckNotNullString("@MethodType", serviceLog.MethodType),
                SqlHelper.CheckNotNullString("@RequestUri", serviceLog.RequestUri),
                SqlHelper.CheckNotNullString("@RequestBody", serviceLog.RequestBody),
                SqlHelper.CheckNotNullString("@EnCodedUrl", serviceLog.EnCodedUrl),
                SqlHelper.CheckNotNullInt("@StatusCode", serviceLog.StatusCode),
                SqlHelper.CheckNotNullString("@ResponseBody", serviceLog.ResponseBody),
                SqlHelper.CheckNotNullBool("@IsActive", serviceLog.IsActive)
            };
                await _repository.ExecuteScalarAsync(
                        "EXEC CreateServiceLog " +
                        "@SourceServiceName, @DestinationService, @MethodType, @RequestUri, @RequestBody, @EnCodedUrl, @StatusCode, @ResponseBody",
                        new PharmacyDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<PharmacyDbContext>(), _config),
                        parameters
                    );
                _logger.LogInformation("{status} {ApiName} {message}","200", "/serviceLog", "Servie Log Sucess");
            }
            catch(Exception ex)
            {
                _logger.LogError("{status} {message} {exception} ", "", "LogServiceCallAsync failed", ex.Message+" - "+ex.StackTrace);
            }
            
        }
    }
}
