using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Wellgistics.Pharmacy.api.Common;
using Wellgistics.Pharmacy.api.Models;
using Azure;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly HttpClientHelper _httpClientHelper;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger, HttpClientHelper httpClientHelper)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClientHelper = httpClientHelper;
    }

    public async Task SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            var postData = new

            {
                apiKey = _configuration["EmailSettings:SendGridApiKey"] ?? "",
                senderEmail = _configuration["EmailSettings:FromEmail"] ?? "",
                senderName = "Wellgistics",
                toEmail = new[] { toEmail },
                subject = subject ?? "",
                body = body ?? "",
                serviceRequestId = 0,
                WorkFlowStepId = 0,
                WorkFlowStepStageId = 0,
                requestType = 2
            };
        
            string endPoint = (_configuration["EmailSettings:baseUrl"]?? "")+ "Integration/SendEmail";
            string fromEmail = _configuration["EmailSettings:FromEmail"]??"";
            string sendGridApiKey = _configuration["EmailSettings:SendGridApiKey"] ?? "";

            await _httpClientHelper.SendRequestAsync<string>(HttpMethod.Post, endPoint, postData);
            _logger.LogInformation("{status} {ApiName} {message}", "200", endPoint, "Send Email Serive Sucess");

        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(@"{status} {message} {exception} ", 500, smtpEx.Message,  smtpEx.StackTrace);
        }
        catch (Exception ex)
        {
            _logger.LogError(@"{status} {message} {exception} ", 500, ex.Message,  ex.StackTrace);
        }
    }
}
