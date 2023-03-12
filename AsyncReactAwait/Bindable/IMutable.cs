using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncReactAwait.Bindable
{

    /// <summary>
    /// Class for representing changable bindable value.
    /// </summary>
    /// <typeparam name="T">Bindable value type.</typeparam>
    public interface IMutable<T> : IBindable<T>
    {

        /// <inheritdoc cref="IBindable{T}.Value"/>
        new T Value { get; set; }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">New value.</param>
        void Set(T value);

        /// <summary>
        /// Sets the value to new value event if they are equal.
        /// </summary>
        /// <param name="value">New value.</param>
        void ForceSet(T value);
    }
}
