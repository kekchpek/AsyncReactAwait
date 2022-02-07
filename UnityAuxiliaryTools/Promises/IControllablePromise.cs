namespace UnityAuxiliaryTools.Promises
{
    public interface IControllablePromise : IBaseControllablePromise, IPromise
    {
        /// <summary>
        /// Mark promise as succeed.
        /// </summary>
        void Success();
    }
    
    public interface IControllablePromise<T> : IBaseControllablePromise, IPromise<T>
    {
        /// <summary>
        /// Mark promise as succeed.
        /// </summary>
        void Success(T result);
    }
}