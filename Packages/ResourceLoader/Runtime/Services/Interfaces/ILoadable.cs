namespace Services.Interfaces
{
    internal interface ILoadable
    {
        void CompleteLoading(object loaded);
        void SetCanceled();
    }
}