using System;

namespace UnityAuxiliaryTools.Promises
{
    /// <summary>
    /// This is a base result independent callbacks for promises.
    /// </summary>
    public interface IBasePromise
    {
        /// <summary>
        /// Callback will be executed in the Unity thread after the promise will fail.
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        IBasePromise OnFail(Action<Exception> callback);
        
        /// <summary>
        /// Callback will be executed in the Unity thread after the promise will success of fail.
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        IBasePromise Finally(Action callback);
    }
}