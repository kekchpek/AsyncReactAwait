using System;

namespace AsyncReactAwait.Bindable
{
    /// <summary>
    /// Non-generic bindable value. 
    /// </summary>
    public interface IBindableRaw : IBindable
    {
        /// <summary>
        /// Current value.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Bind a handler for value changing.
        /// </summary>
        /// <param name="handler">The handler for value changing.</param>
        /// <param name="callImmediately">Apply handler for current value.</param>
        void Bind(Action<object> handler, bool callImmediately = true);

        /// <inheritdoc cref="IBindableRaw.Bind(Action{object}, bool)"/>
        void Bind(Action<object, object> handler);

        /// <summary>
        /// Unbinds a handler for value changing.
        /// </summary>
        /// <param name="handler">The handler for value changing.</param>
        void Unbind(Action<object> handler);

        /// <inheritdoc cref="IBindableRaw.Unbind(Action{object})"/>
        void Unbind(Action<object, object> handler);
    }
}