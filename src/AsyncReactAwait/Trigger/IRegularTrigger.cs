using System;

namespace AsyncReactAwait.Trigger
{

    /// <summary>
    /// The trigger.
    /// </summary>
    [Obsolete]
    public interface IRegularTrigger : ITrigger, ITriggerHandler
    {

    }

    /// <summary>
    /// The value-trigger.
    /// </summary>
    [Obsolete]
    public interface IRegularTrigger<T> : ITrigger<T>, ITriggerHandler<T>
    {

    }
}