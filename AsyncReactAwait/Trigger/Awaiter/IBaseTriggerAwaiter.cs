using System.Runtime.CompilerServices;

namespace AsyncReactAwait.Trigger.Awaiter
{
    /// <summary>
    /// Base inteface for awaiter for trigger.
    /// </summary>
    public interface IBaseTriggerAwaiter<out TConcrete> : INotifyCompletion
        where TConcrete : IBaseTriggerAwaiter<TConcrete>
    {

        /// <summary>
        /// Was trigger activated after awaiting started.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Returns awaiter itself. Required to be used with 'await' keyword.
        /// </summary>
        /// <returns>The awaiter itself.</returns>
        TConcrete GetAwaiter();

        /// <summary>
        /// Gets a container for configured trigger awaiter.
        /// </summary>
        /// <param name="captureContext">Should execution be executed with captured context.</param>
        /// <returns>The container for configured trigger awaiter.</returns>
        TConcrete ConfigureAwaiter(bool captureContext);

    }
}
