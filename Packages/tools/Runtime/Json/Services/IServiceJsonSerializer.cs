namespace Tools.Json.Services
{
    public interface IServiceJsonSerializer
    {
        string Serialize<T>(T json);
        T Deserialize<T>(string json);
    }
}