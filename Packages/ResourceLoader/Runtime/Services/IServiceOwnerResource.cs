namespace Services
{
    public interface IServiceOwnerResource
    {
        void AddOwner(string keyResource, object owner);
        void RemoveOwner(string keyResource, object owner);
        int GetOwnerCount(string keyResource);
    }
}