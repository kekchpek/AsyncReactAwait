using System;

namespace AsyncReactAwait.Promises.Awaiter
{

    /// <summary>
    /// An awaiter for promises with result.
    /// </summary>
    [Obsolete]
    public interface IPromiseAwaiter : IBasePromiseAwaiter<IPromiseAwaiter>
    {
        /// <summary>
        /// Gets the promise result. Throw exception if promise is failed.
        /// </summary>
        void GetResult();
    }

    /// <summary>
    /// An awaiter for promises with result.
    /// </summary>
    [Obsolete]
    public interface IPromiseAwaiter<T> : IBasePromiseAwaiter<IPromiseAwaiter<T>>
    {
        /// <summary>
        /// Gets the promise result. Throw exception if promise is failed.
        /// </summary>
        T GetResult();
    }
}
