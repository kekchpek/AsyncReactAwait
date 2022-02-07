using System;

namespace UnityAuxiliaryTools.Promises
{
    /// <summary>
    /// An async operation with ability to proceed the result. PAY ATTENTION: promises callbacks will be executed in the Unity thread.
    /// </summary>
    /// <typeparam name="T">The type of the result</typeparam>
    public interface IPromise<out T> : IBasePromise
    {
        
        /// <summary>
        /// Callback will be executed in the Unity thread after the promise will success.
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        IPromise<T> OnSuccess(Action<T> callback);
    }
    
    /// <summary>
    /// An async operation with ability to proceed the result. PAY ATTENTION: promises callbacks will be executed in the Unity thread.
    /// </summary>
    public interface IPromise : IBasePromise
    {
        /// <summary>
        /// Callback will be executed in the Unity thread after the promise will success.
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        IPromise OnSuccess(Action callback);
    }
}