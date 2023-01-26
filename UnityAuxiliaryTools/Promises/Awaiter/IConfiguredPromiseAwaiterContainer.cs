using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAuxiliaryTools.Promises.Awaiter
{

    /// <summary>
    /// A container for configured promise awaiter.
    /// </summary>
    public interface IConfiguredPromiseAwaiterContainer
    {

        /// <summary>
        /// Gets configured promise awaiter.
        /// </summary>
        /// <returns>The configured promise awaiter</returns>
        IPromiseAwaiter GetAwaiter();

    }

    /// <summary>
    /// A container for configured promise awaiter.
    /// </summary>
    public interface IConfiguredPromiseAwaiterContainer<T>
    {

        /// <summary>
        /// Gets configured promise awaiter.
        /// </summary>
        /// <returns>The configured promise awaiter</returns>
        IPromiseAwaiter<T> GetAwaiter();

    }
}
