using System;
using System.Threading;

namespace AsyncReactAwait.Trigger.Awaiter
{
    [Obsolete]
    internal class TriggerAwaiter : BaseTriggerAwaiter<ITriggerAwaiter>, ITriggerAwaiter
    {

        private readonly ITriggerHandler _trigger;

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

    [Obsolete]
    internal class TriggerAwaiter<T> : BaseTriggerAwaiter<ITriggerAwaiter<T>>, ITriggerAwaiter<T>
    {

        private readonly ITriggerHandler<T> _trigger;
        private readonly Func<T, bool> _predicate;
        private T? _triggerData;
        private bool _triggerDataSet;

        public TriggerAwaiter(ITriggerHandler<T> trigger, SynchronizationContext? context, Func<T, bool> predicate) : base(context)
        {
            _trigger = trigger ?? throw new ArgumentNullException(nameof(trigger));
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            _trigger.Triggered += TriggerActivated;
        }

        public T GetResult()
        {
            if (_triggerDataSet)
            {
                return _triggerData!;
            }
            throw new Exception("Trigger data not set");
        }

        protected override ITriggerAwaiter<T> GetThis()
        {
            return this;
        }

        private void TriggerActivated(T obj)
        {
            if (_predicate.Invoke(obj))
            {
                _trigger.Triggered -= TriggerActivated;
                _triggerDataSet = true;
                _triggerData = obj;
                Complete();
            }
        }
    }
}
