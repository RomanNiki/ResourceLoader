namespace Services
{
    public interface IServiceCachedResources
    {
        void Add(string key, object resource);
        void Remove(string key);
        bool Has(string key);
        T Get<T>(string key);
    }
}