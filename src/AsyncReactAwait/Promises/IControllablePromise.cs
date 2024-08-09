using System;

namespace AsyncReactAwait.Promises
{

    /// <summary>
    /// An interface to control promise success completition.
    /// </summary>
    [Obsolete]
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
    [Obsolete]
    public interface IControllablePromise<T> : IBaseControllablePromise, IPromise<T>
    {
        /// <summary>
        /// Mark promise as succeed.
        /// </summary>
        void Success(T result);

    }
}