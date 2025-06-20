using System;
using System.Collections.Generic;

namespace AsyncReactAwait.Bindable
{
    internal class BindableDecorator<T, TSource> : IBindable<T>, IBindableRaw
    {
        private readonly IBindable<TSource> _bindable;
        private readonly Func<TSource, T> _predicate;

        private readonly Dictionary<Delegate, Delegate> _handlersMap = new();
        private readonly Dictionary<Delegate, int> _subscriptionCount = new();

        public T Value => _predicate(_bindable.Value);

        object? IBindableRaw.Value => Value;
        
        public event Action? OnAnySubscription;
        public event Action? OnSubscriptionsCleared;

        public void Bind(Action<object?> handler, bool callImmediately = true)
        {
            void NewHandler(TSource x) => handler(_predicate(x));
            _handlersMap.Add(handler, (Action<TSource>)NewHandler);
            _bindable.Bind(NewHandler, callImmediately);

            if (!_subscriptionCount.TryAdd(handler, 1))
                _subscriptionCount[handler]++;
            OnAnySubscription?.Invoke();

        }

        public void Bind(Action<object?, object?> handler)
        {
            void NewHandler(TSource prev, TSource next) => handler(_predicate(prev), _predicate(next));
            _handlersMap.Add(handler, (Action<TSource, TSource>)NewHandler);
            _bindable.Bind(NewHandler);

            if (!_subscriptionCount.TryAdd(handler, 1))
                _subscriptionCount[handler]++;
            OnAnySubscription?.Invoke();
        }

        public void Unbind(Action<object?> handler)
        {
            if (_handlersMap.TryGetValue(handler, out var newHandler))
            {
                _bindable.Unbind((Action<TSource>)newHandler);
            }

            if (_subscriptionCount.ContainsKey(handler))
            {
                if (--_subscriptionCount[handler] <= 0)
                    _subscriptionCount.Remove(handler);
                if (_subscriptionCount.Count == 0)
                    OnSubscriptionsCleared?.Invoke();
            }
        }

        public void Unbind(Action<object?, object?> handler)
        {
            if (_handlersMap.TryGetValue(handler, out var newHandler))
            {
                _bindable.Unbind((Action<TSource, TSource>)newHandler);
            }

            if (_subscriptionCount.ContainsKey(handler))
            {
                if (--_subscriptionCount[handler] <= 0)
                    _subscriptionCount.Remove(handler);
                if (_subscriptionCount.Count == 0)
                    OnSubscriptionsCleared?.Invoke();
            }
        }

        public BindableDecorator(IBindable<TSource> bindable, Func<TSource, T> predicate)
        {
            _bindable = bindable;
            _predicate = predicate;
        }

        public void Bind(Action<T> handler, bool callImmediately = true)
        {
            void NewHandler(TSource x) => handler(_predicate(x));
            _handlersMap.Add(handler, (Action<TSource>)NewHandler);
            _bindable.Bind(NewHandler, callImmediately);

            if (!_subscriptionCount.TryAdd(handler, 1))
                _subscriptionCount[handler]++;

            OnAnySubscription?.Invoke();
        }

        public void Bind(Action handler, bool callImmediately = true)
        {
            _bindable.Bind(handler, callImmediately);

            if (!_subscriptionCount.TryAdd(handler, 1))
                _subscriptionCount[handler]++;
            OnAnySubscription?.Invoke();
        }

        public void Bind(Action<T, T> handler)
        {
            void NewHandler(TSource prev, TSource next) => handler(_predicate(prev), _predicate(next));
            _handlersMap.Add(handler, (Action<TSource, TSource>)NewHandler);
            _bindable.Bind(NewHandler);

            if (!_subscriptionCount.TryAdd(handler, 1))
                _subscriptionCount[handler]++;
            OnAnySubscription?.Invoke();
        }

        public void Unbind(Action<T> handler)
        {
            if (_handlersMap.TryGetValue(handler, out var newHandler))
            {
                _bindable.Unbind((Action<TSource>)newHandler);
            }

            if (_subscriptionCount.ContainsKey(handler))
            {
                if (--_subscriptionCount[handler] <= 0)
                    _subscriptionCount.Remove(handler);
                if (_subscriptionCount.Count == 0)
                    OnSubscriptionsCleared?.Invoke();
            }
        }

        public void Unbind(Action handler)
        {
            _bindable.Unbind(handler);

            if (_subscriptionCount.ContainsKey(handler))
            {
                if (--_subscriptionCount[handler] <= 0)
                    _subscriptionCount.Remove(handler);
                if (_subscriptionCount.Count == 0)
                    OnSubscriptionsCleared?.Invoke();
            }
        }

        public void Unbind(Action<T, T> handler)
        {
            if (_handlersMap.TryGetValue(handler, out var newHandler))
            {
                _bindable.Unbind((Action<TSource, TSource>)newHandler);
            }

            if (_subscriptionCount.ContainsKey(handler))
            {
                if (--_subscriptionCount[handler] <= 0)
                    _subscriptionCount.Remove(handler);
                if (_subscriptionCount.Count == 0)
                    OnSubscriptionsCleared?.Invoke();
            }
        }
    }
}