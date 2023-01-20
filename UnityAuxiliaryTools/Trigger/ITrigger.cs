namespace UnityAuxiliaryTools.Trigger
{
    public interface ITrigger
    {
        void Trigger();
    }

    public interface ITrigger<T>
    {
        void Trigger(T obj);
    }
}