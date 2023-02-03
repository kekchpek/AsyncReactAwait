using AsyncReactAwait.Bindable.Awaiter;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AsyncReactAwait.Bindable.BindableExtensiosn
{

    /// <summary>
    /// Extensions for bindable values.
    /// </summary>
    public static class BindableExtensions
    {

        /// <summary>
        /// Awaits the bindable value becomes specific value.
        /// </summary>
        /// <typeparam name="T">The type of bindable value.</typeparam>
        /// <param name="bindable">The bindable value.</param>
        /// <param name="value">The value to await.</param>
        /// <returns>The awaiter for specified value.</returns>
        public static IBindableAwaiter<T> WillBeEqual<T>(this IBindable<T> bindable, T value)
        {
            return bindable.WillBe(v => v.Equals(value));
        }

    }
}
