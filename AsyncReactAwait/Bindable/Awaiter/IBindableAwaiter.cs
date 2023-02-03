using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AsyncReactAwait.Bindable.Awaiter
{

    /// <summary>
    /// The awaiter for specific bindable value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindableAwaiter<T> : INotifyCompletion, ICriticalNotifyCompletion
    {
        /// <summary>
        /// Indicates that bindable value was changed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Gets awaiter itself.
        /// </summary>
        /// <returns>The awaiter itself.</returns>
        IBindableAwaiter<T> GetAwaiter();

        /// <summary>
        /// Creates configured awaiter for the promise.
        /// </summary>
        /// <param name="captureContext">Should awaited execution be exececuted on captured context.</param>
        /// <returns>The promise awaiter container.</returns>
        IBindableAwaiter<T> ConfigureAwaiter(bool captureContext);

        /// <summary>
        /// Gets the result of awaited process.
        /// </summary>
        /// <returns>The result of awaited process.</returns>
        T GetResult();
    }
}
