namespace Wellgistics.Pharmacy.api.IService
{
    public interface IDopplerService
    {
        Task<string> GetSecretValueAsync(string secretKey);
    }
}
