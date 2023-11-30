using System;
using System.Threading;
using AsyncReactAwait.Trigger.Awaiter;

namespace AsyncReactAwait.Trigger
{

    /// <inheritdoc cref="IRegularTrigger"/>
    public class RegularTrigger : IRegularTrigger
    {

        /// <inheritdoc cref="ITriggerHandler.Triggered"/>
        public event Action? Triggered;

        /// <inheritdoc cref="ITriggerHandler.ConfigureAwaiter(bool)"/>
        public ITriggerAwaiter ConfigureAwaiter(bool captureContext)
        {
            return GetAwaiter().ConfigureAwaiter(captureContext);
        }

        /// <inheritdoc cref="ITriggerHandler.GetAwaiter"/>
        public ITriggerAwaiter GetAwaiter()
        {
            return new TriggerAwaiter(this, SynchronizationContext.Current);
        }

        /// <inheritdoc cref="ITrigger.Trigger"/>
        public void Trigger()
        {
            Triggered?.Invoke();   
        }
    }


    /// <inheritdoc cref="IRegularTrigger{T}"/>
    public class RegularTrigger<T> : RegularTrigger, IRegularTrigger<T>
    {

        /// <inheritdoc cref="ITriggerHandler{T}.Triggered"/>
        public new event Action<T>? Triggered;

        event Action ITriggerHandler.Triggered
        {
            add => base.Triggered += value;
            remove => base.Triggered -= value;
        }


        /// <inheritdoc cref="ITrigger{T}.Trigger(T)"/>
        public void Trigger(T obj)
        {
            Triggered?.Invoke(obj);
            base.Trigger();
        }

        /// <inheritdoc cref="ITriggerHandler{T}.GetAwaiter"/>
        public new ITriggerAwaiter<T> GetAwaiter()
        {
            return new TriggerAwaiter<T>(this, SynchronizationContext.Current, _ => true);
        }

        /// <inheritdoc cref="ITriggerHandler{T}.ConfigureAwaiter(bool)"/>
        public new ITriggerAwaiter<T> ConfigureAwaiter(bool captureContext)
        {
            return GetAwaiter().ConfigureAwaiter(captureContext);
        }

        /// <inheritdoc cref="ITriggerHandler{T}.WillBe"/>
        public ITriggerAwaiter<T> WillBe(Func<T, bool> predicate)
        {
            return new TriggerAwaiter<T>(this, SynchronizationContext.Current, predicate);
        }
    }
}