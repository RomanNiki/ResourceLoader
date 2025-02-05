namespace Services.Interfaces
{
    public interface ILoadable
    {
        void CompleteLoading(object loaded);
        void SetCanceled();
    }
}