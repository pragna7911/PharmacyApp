using Wellgistics.Pharmacy.api.Models;

namespace Wellgistics.Pharmacy.api.IService
{
    public interface IAuthService
    {
        Task<Auth0Token> GetAuthTokenAsync(string url, object postData);
        Task<List<PharmacyEmployee>> Signup(long ncpdp);
        Task<string> ChangePassword(string url, object postData);
        Task<int> UpdatePasswordStatus(UserPassword userPassword);
    }
}
