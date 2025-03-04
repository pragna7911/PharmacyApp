namespace Wellgistics.Pharmacy.api.Models
{
    public class ServiceLogModel
    {
        public string SourceServiceName { get; set; }
        public string DestinationService { get; set; }
        public string MethodType { get; set; }
        public string RequestUri { get; set; }
        public string? RequestBody { get; set; }
        public string? EnCodedUrl { get; set; }
        public int StatusCode { get; set; }
        public string? ResponseBody { get; set; }
        public bool IsActive { get; set; }
    }
}
