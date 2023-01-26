namespace UnityAuxiliaryTools.Trigger.Awaiter
{

    /// <summary>
    /// The contianer for configured trigger awaiter.
    /// </summary>
    public interface IConfiguredTriggerAwaiterContainer
    {

        /// <summary>
        /// Gets configured trigger awaiter.
        /// </summary>
        /// <returns>The configured trigger awaiter.</returns>
        ITriggerAwaiter GetAwaiter();

    }

    /// <summary>
    /// The contianer for configured trigger awaiter.
    /// </summary>
    public interface IConfiguredTriggerAwaiterContainer<T>
    {

        /// <summary>
        /// Gets configured trigger awaiter.
        /// </summary>
        /// <returns>The configured trigger awaiter.</returns>
        ITriggerAwaiter<T> GetAwaiter();

    }
}
