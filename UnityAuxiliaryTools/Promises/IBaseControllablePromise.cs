using System;

namespace UnityAuxiliaryTools.Promises
{
    /// <summary>
    /// The interface to control promise completion.
    /// </summary>
    public interface IBaseControllablePromise : IBasePromise
    {
        /// <summary>
        /// Mark promise as failed.
        /// </summary>
        /// <param name="error">The exception, that is a reason of a failing</param>
        void Fail(Exception error);
    }
}