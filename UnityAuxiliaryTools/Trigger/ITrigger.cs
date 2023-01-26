namespace UnityAuxiliaryTools.Trigger
{

    /// <summary>
    /// The ingerface for trigger activation.
    /// </summary>
    public interface ITrigger
    {

        /// <summary>
        /// Activates the trigger.
        /// </summary>
        void Trigger();
    }

    /// <summary>
    /// The ingerface for value-trigger activation.
    /// </summary>
    public interface ITrigger<T>
    {
        /// <summary>
        /// Activates the trigger and passes some value to it.
        /// </summary>
        /// <param name="obj"></param>
        void Trigger(T obj);
    }
}