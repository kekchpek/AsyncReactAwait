using System.Runtime.CompilerServices;

namespace AsyncReactAwait.Promises.Awaiter
{
    /// <summary>
    /// Base API for promise awaiter.
    /// </summary>
    public interface IBasePromiseAwaiter<out T> : ICriticalNotifyCompletion
        where T : IBasePromiseAwaiter<T>
    {
        /// <summary>
        /// Indicates that promise is completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Gets awaiter itself.
        /// </summary>
        /// <returns>The awaiter itself.</returns>
        T GetAwaiter();

        /// <summary>
        /// Creates configured awaiter for the promise.
        /// </summary>
        /// <param name="captureContext">Should awaited execution be exececuted on captured context.</param>
        /// <returns>The promise awaiter container.</returns>
        T ConfigureAwaiter(bool captureContext);

    }
}
