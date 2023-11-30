using System;
using System.Collections.Generic;
using System.Linq;

namespace AsyncReactAwait.Bindable
{
    /// <summary>
    /// An aggregator for bindable values.
    /// </summary>
    /// <typeparam name="TRes">The aggregated value type.</typeparam>
    public class BindableAggregator<TRes> : IBindable<TRes>
    {
        private readonly IBindableRaw[] _bindableArr;
        private readonly Func<object[], TRes> _aggregator;

        private readonly Dictionary<Action<TRes>, int> _handlers = new();
        private readonly Dictionary<Action, int> _blindHandlers = new();
        private readonly Dictionary<Action<TRes, TRes>, int> _fullHandlers = new();

        private bool _subscribed;

        private TRes? _prevValue;

        /// <inheritdoc cref="IBindable{T}.Value"/>
        public TRes Value => _aggregator.Invoke(_bindableArr.Select(x => x.Value!).ToArray());

        /// <summary>
        /// Constructor for bindable values aggregator.
        /// </summary>
        /// <param name="bindableRaws"></param>
        /// <param name="aggregator"></param>
        public BindableAggregator(IEnumerable<IBindableRaw> bindableRaws, Func<object[], TRes> aggregator)
        {
            if (bindableRaws == null) throw new ArgumentNullException(nameof(bindableRaws));
            _bindableArr = bindableRaws.ToArray();
            _aggregator = aggregator ?? throw new ArgumentNullException(nameof(aggregator));
        }
        
        /// <inheritdoc cref="IBindable{T}.Bind(Action{T}, bool)"/>
        public void Bind(Action<TRes> handler, bool callImmediately = true)
        {
            if (_handlers.ContainsKey(handler))
            {
                _handlers[handler]++;
            }
            else
            {
                _handlers.Add(handler, 1);
            }
            UpdateSourceBinding();
            if (callImmediately)
            {
                handler.Invoke(Value);
            }
        }

        /// <inheritdoc cref="IBindable{T}.Bind(Action, bool)"/>
        public void Bind(Action handler, bool callImmediately = true)
        {
            if (_blindHandlers.ContainsKey(handler))
            {
                _blindHandlers[handler]++;
            }
            else
            {
                _blindHandlers.Add(handler, 1);
            }
            UpdateSourceBinding();
            if (callImmediately)
            {
                handler.Invoke();
            }
        }

        /// <inheritdoc cref="IBindable{T}.Bind(Action{T,T})"/>
        public void Bind(Action<TRes, TRes> handler)
        {
            if (_fullHandlers.ContainsKey(handler))
            {
                _fullHandlers[handler]++;
            }
            else
            {
                _fullHandlers.Add(handler, 1);
            }
            UpdateSourceBinding();
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T})"/>
        public void Unbind(Action<TRes> handler)
        {
            if (!_handlers.ContainsKey(handler))
                return;
            _handlers[handler]--;
            if (_handlers[handler] <= 0)
                _handlers.Remove(handler);
            UpdateSourceBinding();
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action)"/>
        public void Unbind(Action handler)
        {
            if (!_blindHandlers.ContainsKey(handler))
                return;
            _blindHandlers[handler]--;
            if (_blindHandlers[handler] <= 0)
                _blindHandlers.Remove(handler);
            UpdateSourceBinding();
        }

        /// <inheritdoc cref="IBindable{T}.Unbind(Action{T, T})"/>
        public void Unbind(Action<TRes, TRes> handler)
        {
            if (!_fullHandlers.ContainsKey(handler))
                return;
            _fullHandlers[handler]--;
            if (_fullHandlers[handler] <= 0)
                _fullHandlers.Remove(handler);
            UpdateSourceBinding();
        }

        private void UpdateSourceBinding()
        {
            if (_handlers.Any() || _blindHandlers.Any() || _fullHandlers.Any())
            {
                if (!_subscribed)
                {
                    Subscribe();
                }
            }
            else
            {
                if (_subscribed)
                {
                    Unsubscribe();
                }
            }
        }

        private void Subscribe()
        {
            _prevValue = Value;
            foreach (var bindable in _bindableArr)
            {
                bindable.Bind(OnSourceUpdated, false);
                bindable.Bind(OnSourceUpdatedFull);
            }
            _subscribed = true;
        }

        private void OnSourceUpdated(object? value)
        {
            foreach (var keyValuePair in _handlers)
            {
                for (var i = 0; i < keyValuePair.Value; i++)
                {
                    keyValuePair.Key?.Invoke(Value);
                }
            }
            foreach (var keyValuePair in _blindHandlers)
            {
                for (var i = 0; i < keyValuePair.Value; i++)
                {
                    keyValuePair.Key?.Invoke();
                }
            }

            _prevValue = Value;
        }

        private void OnSourceUpdatedFull(object? prevVal, object? nextVal)
        {
            foreach (var keyValuePair in _fullHandlers)
            {
                for (var i = 0; i < keyValuePair.Value; i++)
                {
                    keyValuePair.Key?.Invoke(_prevValue!, Value);
                }
            }

            _prevValue = Value;
        }

        private void Unsubscribe()
        {
            foreach (var bindable in _bindableArr)
            {
                bindable.Unbind(OnSourceUpdated);
                bindable.Unbind(OnSourceUpdatedFull);
            }
            _subscribed = false;
        }
    }
}