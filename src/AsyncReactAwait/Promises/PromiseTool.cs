using System;

namespace AsyncReactAwait.Promises
{
    /// <summary>
    /// Auxiliary tools for promises.
    /// </summary>
    public static class PromiseTool
    {

        private static readonly IControllablePromise SuccessfulPromise;

        static PromiseTool()
        {
            SuccessfulPromise = new ControllablePromise();
            SuccessfulPromise.Success();
        }
        
        /// <summary>
        /// Awaits all passed promises.
        /// </summary>
        /// <param name="promises">Promises to await.</param>
        public static async IPromise AwaitAll(params IPromise[] promises)
        {
            foreach (var promise in promises)
            {
                await promise;
            }
        }
        
        /// <summary>
        /// Awaits any of passed promise.
        /// </summary>
        /// <param name="promises">Promises to await.</param>
        /// <returns>The promise indicates the awaiting.</returns>
        public static IPromise AwaitAny(params IPromise[] promises)
        {
            IControllablePromise awaitAnyPromise = new ControllablePromise();
            var anyCompleted = false;

            void Completion()
            {
                if (!anyCompleted)
                {
                    anyCompleted = true;
                    awaitAnyPromise.Success();
                }
            }

            foreach (var promise in promises)
            {
                promise.GetAwaiter().OnCompleted(Completion);
            }

            return awaitAnyPromise;
        }

        /// <summary>
        /// Gets a predefined successful promise.
        /// </summary>
        /// <returns>Predefined successful promise.</returns>
        public static IPromise GetSuccessful()
        {
            return SuccessfulPromise;
        }

        /// <summary>
        /// Creates a successful promise for specified results.
        /// </summary>
        /// <param name="result">The result of a promise.</param>
        /// <typeparam name="T">The type of a promise result.</typeparam>
        /// <returns></returns>
        public static IPromise<T> GetSuccessful<T>(T result)
        {
            var promise = new ControllablePromise<T>();
            promise.Success(result);
            return promise;
        }

        /// <summary>
        /// Creates a failed promise with specified error.
        /// </summary>
        /// <param name="exception">The exception to set to promise.</param>
        /// <returns>A failed promise.</returns>
        public static IPromise GetFailed(Exception exception)
        {
            var promise = new ControllablePromise();
            promise.Fail(exception);
            return promise;
        }

        /// <summary>
        /// Creates a failed promise with specified error.
        /// </summary>
        /// <param name="exception">The exception to set to promise.</param>
        /// <typeparam name="T">The type of promise result.</typeparam>
        /// <returns>A failed promise.</returns>
        public static IPromise<T> GetFailed<T>(Exception exception)
        {
            var promise = new ControllablePromise<T>();
            promise.Fail(exception);
            return promise;
        }
    }
}