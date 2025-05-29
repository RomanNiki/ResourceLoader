namespace Services.Load
{
    internal interface ILoadable
    {
        void CompleteLoading(object loaded);
        void SetCanceled();
    }
}