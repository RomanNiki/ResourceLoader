namespace Containers
{
    public interface ILoadable
    {
        void CompleteLoading(object loaded);
        void SetCanceled();
    }
}