using System;
using System.Collections.Generic;
using System.Text;

namespace UnityAuxiliaryTools.Trigger.Awaiter
{
    internal class ConfiguredTriggerAwaiterContainer : IConfiguredTriggerAwaiterContainer
    {

        private readonly ITriggerAwaiter _triggerAwaiter;

        public ConfiguredTriggerAwaiterContainer(ITriggerAwaiter awaiter)
        {
            _triggerAwaiter = awaiter;
        }

        public ITriggerAwaiter GetAwaiter()
        {
            return _triggerAwaiter;
        }
    }
    internal class ConfiguredTriggerAwaiterContainer<T> : IConfiguredTriggerAwaiterContainer<T>
    {

        private readonly ITriggerAwaiter<T> _triggerAwaiter;

        public ConfiguredTriggerAwaiterContainer(ITriggerAwaiter<T> awaiter)
        {
            _triggerAwaiter = awaiter;
        }

        public ITriggerAwaiter<T> GetAwaiter()
        {
            return _triggerAwaiter;
        }
    }
}
