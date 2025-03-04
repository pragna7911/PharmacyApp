using static StackyonService;

public interface IStackyonService
{
    Task<StackyonConnectResponse> CallServiceApi(object requestBody);
    Task<string> Decrypt(string encodedUrl);

}
