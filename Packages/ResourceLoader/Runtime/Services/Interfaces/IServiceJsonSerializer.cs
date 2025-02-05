namespace Services.Interfaces
{
    public interface IServiceJsonSerializer
    {
        string Serialize<T>(T json);
        T Deserialize<T>(string json);
    }
}