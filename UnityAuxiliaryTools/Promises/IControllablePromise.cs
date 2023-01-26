namespace UnityAuxiliaryTools.Promises
{

    /// <summary>
    /// An interface to control promise success completition.
    /// </summary>
    public interface IControllablePromise : IBaseControllablePromise, IPromise
    {
        /// <summary>
        /// Mark promise as succeed.
        /// </summary>
        void Success();
    }

    /// <summary>
    /// An interface to control promise success completition.
    /// </summary>
    public interface IControllablePromise<T> : IBaseControllablePromise, IPromise<T>
    {
        /// <summary>
        /// Mark promise as succeed.
        /// </summary>
        void Success(T result);

    }
}