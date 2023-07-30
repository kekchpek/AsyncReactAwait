namespace AsyncReactAwait.Promises.Extensions
{
    /// <summary>
    /// Extensions for promises.
    /// </summary>
    public static class PromiseExtensions
    {
        /// <summary>
        /// Wraps a value promise inside the non-generic one.
        /// </summary>
        /// <param name="promise">The promise to wrap.</param>
        /// <typeparam name="T">The return type of wrapped promise.</typeparam>
        /// <returns>The non-generic promise.</returns>
        public static IPromise Wrap<T>(this IPromise<T> promise)
        {
            var outcome = new ControllablePromise();
            promise.OnSuccess(_ => outcome.Success());
            return outcome;
        }
    }
}