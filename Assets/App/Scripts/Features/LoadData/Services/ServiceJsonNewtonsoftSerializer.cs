using Newtonsoft.Json;
using Services.Interfaces;

namespace App.Scripts.Features.LoadData.Services
{
    public class ServiceJsonNewtonsoftSerializer : IServiceJsonSerializer
    {
        public string Serialize<T>(T json)
        {
            return JsonConvert.SerializeObject(json);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}