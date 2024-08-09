using System;

namespace AsyncReactAwait.Promises
{
    /// <summary>
    /// This is a base result independent callbacks for promises.
    /// </summary>
    [Obsolete]
    public interface IBasePromise
    {

        /// <summary>
        /// Returns true if promise is completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Callback will be executed after the promise will fail.
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        IBasePromise OnFail(Action<Exception> callback);
        
        /// <summary>
        /// Callback will be executed after the promise will success of fail.
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        IBasePromise Finally(Action callback);
    }
}