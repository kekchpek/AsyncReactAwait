using System;
using System.Threading;
using UnityEngine;

namespace UnityAuxiliaryTools.Trigger.Awaiter
{
    internal class TriggerAwaiter : BaseTriggerAwaiter, ITriggerAwaiter
    {
        public TriggerAwaiter(ITriggerHandler trigger, SynchronizationContext context) : base(trigger, context)
        {
        }

        public void GetResult()
        {
        }
    }

    internal class TriggerAwaiter<T> : BaseTriggerAwaiter, ITriggerAwaiter<T>
    {

        private readonly ITriggerHandler<T> _trigger;

        private T _triggerData;
        private bool _triggerDataSet = false;

        public TriggerAwaiter(ITriggerHandler<T> trigger, SynchronizationContext context) : base(trigger, context)
        {
            _trigger = trigger;
            _trigger.Triggered += TriggerActivated;
        }

        public T GetResult()
        {
            if (_triggerDataSet)
            {
                return _triggerData;
            }
            throw new Exception("Trigger data not set");
        }

        private void TriggerActivated(T obj)
        {
            _trigger.Triggered -= TriggerActivated;
            _triggerDataSet = true;
            _triggerData = obj;
        }
    }
}
