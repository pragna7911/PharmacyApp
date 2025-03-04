using System.Diagnostics;
using System.Text;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Layouts;

namespace Wellgistics.Pharmacy.api.Common
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Capture API name and request method
            var apiName = context.Request.Path;
            var method = context.Request.Method;
            var requestInfo = "";//Convert.ToString(context.Request.QueryString) is not null ? $"Querytring:{context.Request.QueryString}" : "";
            //if (method.ToLower() == "post")
            //{
            //    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            //    {
            //        var body = await reader.ReadToEndAsync();
            //        requestInfo = "Request Body: " + body;
            //    }
            //}
            // Log before request
            //_logger.LogInformation("Started Request - API: {ApiName} | Method: {RequestMethod}", apiName, method);

            // Proceed with the request
            await _next(context);

            // Measure the total time for the request
            stopwatch.Stop();
            var totalTime = stopwatch.ElapsedMilliseconds;

            // Log after request with the total time
            //_logger.LogInformation("Completed Request - API: {ApiName} | Method: {RequestMethod} | TotalTime: {TotalTime}ms | RequestInfo: {RequestInfo}",
            // apiName, method, totalTime, requestInfo);

            // Log custom properties for NLog
            _logger.LogInformation("{ApiName} {RequestMethod} {TotalTime} {RequestInfo} {status}", apiName, method, totalTime, requestInfo, context.Response.StatusCode);
            //using (_logger.BeginScope(new Dictionary<string, object>
            //{
            //    ["ApiName"] = apiName,
            //    ["RequestMethod"] = method,
            //    ["TotalTime"] = totalTime,
            //    ["RequestInfo"] = requestInfo
            //}))
            //{
            //    // This will now include the custom properties
            //    //_logger.LogInformation("Request completed with custom properties.");
            //    _logger.LogInformation("{ApiName} {RequestMethod} {TotalTime} {RequestInfo} ", apiName, method, totalTime, requestInfo);
                    
            //}
        }
    }



}
