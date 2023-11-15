using System;
using System.Threading;

namespace AsyncReactAwait.Trigger.Awaiter
{
    internal class TriggerAwaiter : BaseTriggerAwaiter<ITriggerAwaiter>, ITriggerAwaiter
    {

        private ITriggerHandler _trigger;

        public TriggerAwaiter(ITriggerHandler trigger, SynchronizationContext context) : base(context)
        {
            _trigger = trigger;
            _trigger.Triggered += OnTriggered;
        }

        private void OnTriggered()
        {
            _trigger.Triggered -= OnTriggered;
            Complete();
        }

        public void GetResult()
        {
        }

        protected override ITriggerAwaiter GetThis()
        {
            return this;
        }
    }

    internal class TriggerAwaiter<T> : BaseTriggerAwaiter<ITriggerAwaiter<T>>, ITriggerAwaiter<T>
    {

        private readonly ITriggerHandler<T> _trigger;
        private readonly Func<T, bool> _predicate;
        private T _triggerData;
        private bool _triggerDataSet = false;

        public TriggerAwaiter(ITriggerHandler<T> trigger, SynchronizationContext context, Func<T, bool> predicate) : base(context)
        {
            _trigger = trigger;
            _predicate = predicate;
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

        protected override ITriggerAwaiter<T> GetThis()
        {
            return this;
        }

        private void TriggerActivated(T obj)
        {
            if (_predicate == null || _predicate.Invoke(obj))
            {
                _trigger.Triggered -= TriggerActivated;
                _triggerDataSet = true;
                _triggerData = obj;
                Complete();
            }
        }
    }
}
