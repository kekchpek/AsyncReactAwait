using System;
using System.Runtime.CompilerServices;

namespace UnityAuxiliaryTools.Promises.Awaiter
{

    /// <summary>
    /// An awaiter for promises with result.
    /// </summary>
    public interface IPromiseAwaiter : IBasePromiseAwaiter
    {
        /// <summary>
        /// Gets the promise result. Throw exception if promise is failed.
        /// </summary>
        void GetResult();
    }

    /// <summary>
    /// An awaiter for promises with result.
    /// </summary>
    public interface IPromiseAwaiter<T> : IBasePromiseAwaiter
    {
        /// <summary>
        /// Gets the promise result. Throw exception if promise is failed.
        /// </summary>
        T GetResult();
    }
}
