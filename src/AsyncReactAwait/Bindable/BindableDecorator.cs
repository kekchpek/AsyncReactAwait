using System;
using System.Collections.Generic;

namespace AsyncReactAwait.Bindable
{
    internal class BindableDecorator<T, TSource> : IBindable<T>
    {
        private readonly IBindable<TSource> _bindable;
        private readonly Func<TSource, T> _predicate;

        private readonly Dictionary<Delegate, Delegate> _handlersMap = new();

        public T Value => _predicate(_bindable.Value);

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
        }

        public void Bind(Action handler, bool callImmediately = true)
        {
            _bindable.Bind(handler, callImmediately);
        }

        public void Bind(Action<T, T> handler)
        {
            void NewHandler(TSource prev, TSource next) => handler(_predicate(prev), _predicate(next));
            _handlersMap.Add(handler, (Action<TSource, TSource>)NewHandler);
            _bindable.Bind(NewHandler);
        }

        public void Unbind(Action<T> handler)
        {
            if (_handlersMap.TryGetValue(handler, out var newHandler))
            {
                _bindable.Unbind((Action<TSource>)newHandler);
            }
        }

        public void Unbind(Action handler)
        {
            _bindable.Unbind(handler);
        }

        public void Unbind(Action<T, T> handler)
        {
            if (_handlersMap.TryGetValue(handler, out var newHandler))
            {
                _bindable.Unbind((Action<TSource, TSource>)newHandler);
            }
        }
    }
}