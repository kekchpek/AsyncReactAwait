using AsyncReactAwait.Trigger;
using AsyncReactAwait.Trigger.Awaiter;
using AsyncReactAwait.Trigger.Extensions;
using System;

namespace AsyncReactAwait.Trigger.Extensions
{

    /// <summary>
    /// Extensions for triggers.
    /// </summary>
    public static class TriggerExtensions
    {


        /// <summary>
        /// Awaits until bindable value becomes some specific value.
        /// </summary>
        /// <param name="trigger">The trigger to extend.</param>
        /// <param name="value">The value to await.</param>
        /// <returns>The awater for specified value.</returns>
        public static ITriggerAwaiter<T> WillBeEqual<T>(this ITriggerHandler<T> trigger, T value)
        {
            return trigger.WillBe(x => x.Equals(value));
        }

    }
}
