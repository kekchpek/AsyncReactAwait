using System;
using System.Runtime.CompilerServices;
using AsyncReactAwait.Promises.AsyncMethodBuilder;
using AsyncReactAwait.Promises.Awaiter;

namespace AsyncReactAwait.Promises
{
    /// <summary>
    /// An async operation with ability to proceed the result.
    /// </summary>
    /// <typeparam name="T">The type of the result</typeparam>
    [AsyncMethodBuilder(typeof(AsyncPromiseMethodBuilder<>))]
    public interface IPromise<T> : IBasePromise
    {

        /// <summary>
        /// Creates configured awaiter for the promise.
        /// </summary>
        /// <param name="continueOnCapturedContext">Should awaited execution be exececuted on captured context.</param>
        /// <returns>The promise awaiter container</returns>
        IPromiseAwaiter<T> ConfigureAwait(bool continueOnCapturedContext);

        /// <summary>
        /// Gets an awaiter for promise async execution.
        /// </summary>
        /// <returns>The promise awaiter</returns>
        IPromiseAwaiter<T> GetAwaiter();

        /// <summary>
        /// Callback will be executed after the promise will success.
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        IPromise<T> OnSuccess(Action<T> callback);

        /// <summary>
        /// Obtaining a result if promise is completed.
        /// </summary>
        /// <param name="result">The result of the promise.</param>
        /// <returns>Returns true if result can be obtained. False - otherwise.</returns>
        bool TryGetResult(out T? result);
    }


    /// <summary>
    /// An async operation with ability to proceed the result.
    /// </summary>
    [AsyncMethodBuilder(typeof(AsyncPromiseMethodBuilder))]
    public interface IPromise : IBasePromise
    {
        /// <summary>
        /// Creates configured awaiter for the promise.
        /// </summary>
        /// <param name="captureContext">Should awaited execution be exececuted on captured context.</param>
        /// <returns>The promise awaiter container.</returns>
        IPromiseAwaiter ConfigureAwait(bool captureContext);

        /// <summary>
        /// Gets an awaiter for promise async execution.
        /// </summary>
        /// <returns>The promise awaiter</returns>
        IPromiseAwaiter GetAwaiter();

        /// <summary>
        /// Callback will be executed after the promise will success.
        /// </summary>
        /// <param name="callback">A callback to execute</param>
        IPromise OnSuccess(Action callback);

        /// <summary>
        /// Throws an exception if promise is failed.
        /// </summary>
        void ThrowIfFailed();
    }
}