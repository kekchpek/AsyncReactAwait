using System;

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
            promise.OnFail(e => outcome.Fail(e));
            return outcome;
        }

        /// <summary>
        /// Wraps a non-result to a promise and evaluate it with a result.
        /// </summary>
        /// <param name="promise">The promise to wrap.</param>
        /// <param name="resultGetter">The result getter, that will be called, when promise will completed.</param>
        /// <typeparam name="T">The return type of wrapped promise.</typeparam>
        /// <returns>The promise with a result.</returns>
        public static IPromise<T> Wrap<T>(this IPromise promise, Func<T> resultGetter)
        {
            if (promise == null) throw new ArgumentNullException(nameof(promise));
            if (resultGetter == null) throw new ArgumentNullException(nameof(resultGetter));
            
            var outcome = new ControllablePromise<T>();
            promise.OnSuccess(() => outcome.Success(resultGetter.Invoke()));
            promise.OnFail(e => outcome.Fail(e));
            return outcome;
        }

        /// <summary>
        /// Converts a promise value to other one.
        /// </summary>
        /// <param name="promise">The promise to convert.</param>
        /// <param name="converter">The converter for source promise result.</param>
        /// <typeparam name="TSource">The type of a promise result.</typeparam>
        /// <typeparam name="TRes">The type of conversion result.</typeparam>
        /// <returns></returns>
        public static IPromise<TRes> Convert<TSource, TRes>(this IPromise<TSource> promise, Func<TSource, TRes> converter)
        {
            if (promise == null) throw new ArgumentNullException(nameof(promise));
            if (converter == null) throw new ArgumentNullException(nameof(converter));
            
            var outcome = new ControllablePromise<TRes>();
            promise.OnSuccess(x => outcome.Success(converter.Invoke(x)));
            promise.OnFail(e => outcome.Fail(e));
            return outcome;
        }
    }
}