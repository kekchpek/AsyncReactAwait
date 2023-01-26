namespace UnityAuxiliaryTools.Trigger
{

    /// <summary>
    /// The trigger.
    /// </summary>
    public interface IRegularTrigger : ITrigger, ITriggerHandler
    {

    }

    /// <summary>
    /// The value-trigger.
    /// </summary>
    public interface IRegularTrigger<T> : ITrigger<T>, ITriggerHandler<T>
    {

    }
}