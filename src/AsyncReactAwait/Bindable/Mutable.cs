using System;

namespace AsyncReactAwait.Bindable
{

    /// <inheritdoc cref="IMutable{T}"/>
    public class Mutable<T> : IMutable<T>, IBindableRaw
    {

        private T _value;

        private IBindable<T>? _proxiedObject;

        private event Action<T>? OnChange;
        private event Action<object?>? OnChangeRaw;
        private event Action? OnChangeBlind;
        private event Action<T, T>? OnChangeFull;
        private event Action<object?, object?>? OnChangeFullRaw;
        public event Action? OnAnySubscription;
        public event Action? OnSubscriptionsCleared;

        /// <inheritdoc cref="IMutable{T}.Value"/>
        public T Value
        {
            get => _value;
            set
            {
                StopProxying();
                if (Equals(_value, value))
                    return;
                ForceSetInternal(value);
            }
        }

        object? IBindableRaw.Value => Value;

        T IBindable<T>.Value => _value;

        /// <summary>
        /// Default constructor to create mutable value.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        public Mutable(T initialValue = default!)
        {
            _value = initialValue;
        }

        /// <inheritdoc />
        public void Proxy(IBindable<T> valueSource)
        {
            StopProxying();
            _proxiedObject = valueSource;
            _proxiedObject.Bind(OnProxyChanged);
        }

        private void OnProxyChanged(T proxyValue)
        {
            ForceSetInternal(proxyValue);
        }

        /// <inheritdoc />
        public void StopProxying()
        {
            if (_proxiedObject == null)
                return;
            
            _proxiedObject.Unbind(OnProxyChanged);
            _proxiedObject = null;
        }

        /// <inheritdoc />
        public void ForceSet(T value)
        {
            StopProxying();
            ForceSetInternal(value);
        }

        private void ForceSetInternal(T value)
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
        public void Bind(Action<object?> handler, bool callImmediately = true)
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
            OnAnySubscription?.Invoke();
        }

        /// <inheritdoc />
        public void Bind(Action<object?, object?> handler)
        {
            OnChangeFullRaw += handler ?? throw new ArgumentNullException(nameof(handler));
            OnAnySubscription?.Invoke();
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
            OnAnySubscription?.Invoke();
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
            OnAnySubscription?.Invoke();
        }

        /// <inheritdoc />
        public void Bind(Action<T, T> handler)
        {
            OnChangeFull += handler;
            OnAnySubscription?.Invoke();
        }

        /// <inheritdoc />
        public void Unbind(Action<object?> handler)
        {
            if (!AnyListeners())
                return;
            OnChangeRaw -= handler;
            if (!AnyListeners())
                OnSubscriptionsCleared?.Invoke();
        }

        /// <inheritdoc />
        public void Unbind(Action<object?, object?> handler)
        {
            if (!AnyListeners())
                return;
            OnChangeFullRaw -= handler;
            if (!AnyListeners())
                OnSubscriptionsCleared?.Invoke();
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T})"/>
        public void Unbind(Action<T> handler)
        {
            if (!AnyListeners())
                return;
            OnChange -= handler;
            if (!AnyListeners())
                OnSubscriptionsCleared?.Invoke();
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action)"/>
        public void Unbind(Action handler)
        {
            if (!AnyListeners())
                return;
            OnChangeBlind -= handler;
            if (!AnyListeners())
                OnSubscriptionsCleared?.Invoke();
        }

        /// <inheritdoc />
        public void Unbind(Action<T, T> handler)
        {
            if (!AnyListeners())
                return;
            OnChangeFull -= handler;
            if (!AnyListeners())
                OnSubscriptionsCleared?.Invoke();
        }

        private bool AnyListeners() =>
            OnChangeFull != null || OnChange != null || OnChangeBlind != null ||
            OnChangeRaw != null || OnChangeFullRaw != null;
    }
}
