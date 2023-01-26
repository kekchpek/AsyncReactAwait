using System;
using System.Threading;
using UnityAuxiliaryTools.Trigger.Awaiter;
using UnityEngine;

namespace UnityAuxiliaryTools.Trigger
{

    /// <inheritdoc cref="IRegularTrigger"/>
    public class RegularTrigger : IRegularTrigger
    {

        /// <inheritdoc cref="ITriggerHandler.Triggered"/>
        public event Action Triggered;

        /// <inheritdoc cref="ITriggerHandler.ConfigureAwaiter(bool)"/>
        public IConfiguredTriggerAwaiterContainer ConfigureAwaiter(bool captureContext)
        {
            if (captureContext)
            {
                return new ConfiguredTriggerAwaiterContainer(GetAwaiter());
            }
            else
            {
                return new ConfiguredTriggerAwaiterContainer(new TriggerAwaiter(this, null));
            }
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
        public new event Action<T> Triggered;

        event Action ITriggerHandler.Triggered
        {
            add
            {
                base.Triggered += value;
            }

            remove
            {
                base.Triggered -= value;
            }
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
            return new TriggerAwaiter<T>(this, SynchronizationContext.Current);
        }

        /// <inheritdoc cref="ITriggerHandler{T}.ConfigureAwaiter(bool)"/>
        public new IConfiguredTriggerAwaiterContainer<T> ConfigureAwaiter(bool captureContext)
        {
            if (captureContext)
            {
                return new ConfiguredTriggerAwaiterContainer<T>(GetAwaiter());
            }
            else
            {
                return new ConfiguredTriggerAwaiterContainer<T>(new TriggerAwaiter<T>(this, null));
            }
        }
    }
}