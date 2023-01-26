namespace UnityAuxiliaryTools.Trigger
{
    public interface IRegularTrigger : ITrigger, ITriggerHandler
    {

    }

    public interface IRegularTrigger<T> : ITrigger<T>, ITriggerHandler<T>
    {

    }
}