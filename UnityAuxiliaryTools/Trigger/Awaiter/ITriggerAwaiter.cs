using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAuxiliaryTools.Trigger.Awaiter
{

    /// <summary>
    /// Interface for awaiter for trigger.
    /// </summary>
    public interface ITriggerAwaiter : IBaseTriggerAwaiter
    {

        /// <summary>
        /// Do nothing. Required for await/async compatibility.
        /// </summary>
        void GetResult();

    }

    /// <summary>
    /// Interface for awaiter for trigger.
    /// </summary>
    public interface ITriggerAwaiter<T> : IBaseTriggerAwaiter
    {

        /// <summary>
        /// Returns trigger activation payload.
        /// </summary>
        T GetResult();

    }
}
