using System;
using System.Diagnostics.CodeAnalysis;

namespace AsyncReactAwait.Bindable
{

    /// <inheritdoc cref="IMutable{T}"/>
    public class Mutable<T> : IMutable<T>, IBindableRaw
    {

        [AllowNull]
        private T _value;

        private event Action<T> OnChange;
        private event Action<object> OnChangeRaw;
        private event Action OnChangeBlind;
        private event Action<T, T> OnChangeFull;
        private event Action<object, object> OnChangeFullRaw;

        /// <inheritdoc cref="IMutable{T}.Value"/>
        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                    return;
                ForceSet(value);
            }
        }

        object IBindableRaw.Value => Value;

        T IBindable<T>.Value => _value;

        /// <summary>
        /// Default constructor to create mutable value.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        public Mutable([AllowNull] T initialValue = default)
        {
            _value = initialValue;
        }

        /// <inheritdoc />
        public void ForceSet(T value)
        {
            var previousVal = _value;
            _value = value;
            OnChangeBlind?.Invoke();
            OnChangeFullRaw?.Invoke(previousVal, _value);
            OnChangeFull?.Invoke(previousVal, _value);
            OnChangeRaw?.Invoke(_value);
            OnChange?.Invoke(_value);
        }
        
        /// <inheritdoc />
        public void Bind(Action<object> handler, bool callImmediately = true)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (callImmediately)
            {
                handler.Invoke(_value);
            }
            OnChangeRaw += handler;
        }

        /// <inheritdoc />
        public void Bind(Action<object, object> handler)
        {
            OnChangeFullRaw += handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc />
        public void Unbind(Action<object> handler)
        {
            OnChangeRaw -= handler;
        }

        /// <inheritdoc />
        public void Unbind(Action<object, object> handler)
        {
            OnChangeFullRaw -= handler;
        }

        /// <inheritdoc cref="IBindable{T}.Bind(Action{T}, bool)"/>
        /// <exception cref="ArgumentNullException"></exception>
        public void Bind(Action<T> handler, bool callImmediately = true)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (callImmediately)
            {
                handler.Invoke(_value);
            }
            OnChange += handler;
        }

        /// <inheritdoc cref="IBindable{T}.Bind(Action, bool)"/>
        /// <exception cref="ArgumentNullException"></exception>
        public void Bind(Action handler, bool callImmediately = true)
        {
            OnChangeBlind += handler ?? throw new ArgumentNullException(nameof(handler));

            if (callImmediately)
            {
                handler.Invoke();
            }
        }

        /// <inheritdoc />
        public void Bind(Action<T, T> handler)
        {
            OnChangeFull += handler;
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T})"/>
        public void Unbind(Action<T> handler)
        {
            OnChange -= handler;
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action)"/>
        public void Unbind(Action handler)
        {
            OnChangeBlind -= handler;
        }

        /// <inheritdoc />
        public void Unbind(Action<T, T> handler)
        {
            OnChangeFull -= handler;
        }
    }
}
