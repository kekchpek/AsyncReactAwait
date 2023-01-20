namespace UnityAuxiliaryTools.Trigger.Factory
{
    public interface IRegularTrigger : ITrigger, ITriggerHandler
    {
        
    }

    public interface IRegularTrigger<T> : ITrigger<T>, ITriggerHandler<T>
    {

    }
}