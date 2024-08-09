using System;

namespace AsyncReactAwait.Trigger.Awaiter
{

    /// <summary>
    /// Interface for awaiter for trigger.
    /// </summary>
    [Obsolete]
    public interface ITriggerAwaiter : IBaseTriggerAwaiter<ITriggerAwaiter>
    {

        /// <summary>
        /// Do nothing. Required for await/async compatibility.
        /// </summary>
        void GetResult();

    }

    /// <summary>
    /// Interface for awaiter for trigger.
    /// </summary>
    [Obsolete]
    public interface ITriggerAwaiter<T> : IBaseTriggerAwaiter<ITriggerAwaiter<T>>
    {

        /// <summary>
        /// Returns trigger activation payload.
        /// </summary>
        T GetResult();

    }
}
