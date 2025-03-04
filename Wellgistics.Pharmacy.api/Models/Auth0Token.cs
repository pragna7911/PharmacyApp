namespace Wellgistics.Pharmacy.api.Models
{
    public class Auth0Token
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
    }
    public class Auth0SignupResponse
    {
        public string _id { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string password { get; set; }
    }
    public class RefreshToken
    {
        public string refresh_token { get; set; }
        
    }
}
