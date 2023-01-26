using System;
using UnityAuxiliaryTools.Trigger.Awaiter;

namespace UnityAuxiliaryTools.Trigger
{

    /// <summary>
    ///  The interface for handling trigger activation.
    /// </summary>
    public interface ITriggerHandler
    {

        /// <summary>
        /// Fired on trigger actiovation.
        /// </summary>
        event Action Triggered;

        /// <summary>
        /// Gets an awaiter for trigger activation.
        /// </summary>
        /// <returns>The trigger awaiter.</returns>
        ITriggerAwaiter GetAwaiter();

        /// <summary>
        /// Gets a container for configured trigger awaiter.
        /// </summary>
        /// <param name="captureContext">Should execution be executed with captured context.</param>
        /// <returns>The container for configured trigger awaiter.</returns>
        IConfiguredTriggerAwaiterContainer ConfigureAwaiter(bool captureContext);
    }

    /// <summary>
    /// The interface for handling value-trigger activation.
    /// </summary>
    public interface ITriggerHandler<T> : ITriggerHandler
    {
        /// <summary>
        /// Fired on value-trigger actiovation.
        /// </summary>
        new event Action<T> Triggered;

        /// <summary>
        /// Gets an awaiter for value-trigger activation.
        /// </summary>
        /// <returns>The value-trigger awaiter.</returns>
        new ITriggerAwaiter<T> GetAwaiter();

        /// <summary>
        /// Gets a container for configured value-trigger awaiter.
        /// </summary>
        /// <param name="captureContext">Should execution be executed with captured context.</param>
        /// <returns>The container for configured value-trigger awaiter.</returns>
        new IConfiguredTriggerAwaiterContainer<T> ConfigureAwaiter(bool captureContext);
    }
}