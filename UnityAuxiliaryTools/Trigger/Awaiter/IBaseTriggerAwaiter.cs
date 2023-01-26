using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace UnityAuxiliaryTools.Trigger.Awaiter
{
    /// <summary>
    /// Base inteface for awaiter for trigger.
    /// </summary>
    public interface IBaseTriggerAwaiter : INotifyCompletion
    {

        /// <summary>
        /// Was trigger activated after awaiting started.
        /// </summary>
        bool IsCompleted { get; }

    }
}
