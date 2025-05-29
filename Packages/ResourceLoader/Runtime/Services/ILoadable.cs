namespace Services
{
    internal interface ILoadable
    {
        void CompleteLoading(object loaded);
        void SetCanceled();
    }
}