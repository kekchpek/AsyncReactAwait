using System;
using System.Collections.Generic;
using NSubstitute;
using AsyncReactAwait.Promises;

namespace SocialDemo.Code.Tests.EditMode.TestUtils
{
    public static class TestPromisesStaticFactory
    {
        public static IPromise CreatePromise(out WeakReference<List<Action>> successCallbacksRef,
            out WeakReference<List<Action<Exception>>> errorCallbacksRef,
            out WeakReference<List<Action>> finallyCallbacksRef)
        {
            var promise = Substitute.For<IPromise>();
            var successCallbacks = new List<Action>();
            successCallbacksRef = new WeakReference<List<Action>>(successCallbacks);
            var errorCallbacks = new List<Action<Exception>>();
            errorCallbacksRef = new WeakReference<List<Action<Exception>>>(errorCallbacks);
            var finallyCallbacks = new List<Action>();
            finallyCallbacksRef = new WeakReference<List<Action>>(finallyCallbacks);

            promise.OnSuccess(Arg.Do<Action>(x => successCallbacks.Add(x))).Returns(promise);
            promise.Finally(Arg.Do<Action>(x => finallyCallbacks.Add(x))).Returns(promise);
            promise.OnFail(Arg.Do<Action<Exception>>(x => errorCallbacks.Add(x))).Returns(promise);

            return promise;
        }
        
        public static IPromise<T> CreatePromise<T>(out WeakReference<List<Action<T>>> successCallbacksRef,
            out WeakReference<List<Action<Exception>>> errorCallbacksRef,
            out WeakReference<List<Action>> finallyCallbacksRef)
        {
            var promise = Substitute.For<IPromise<T>>();
            var successCallbacks = new List<Action<T>>();
            successCallbacksRef = new WeakReference<List<Action<T>>>(successCallbacks);
            var errorCallbacks = new List<Action<Exception>>();
            errorCallbacksRef = new WeakReference<List<Action<Exception>>>(errorCallbacks);
            var finallyCallbacks = new List<Action>();
            finallyCallbacksRef = new WeakReference<List<Action>>(finallyCallbacks);

            promise.OnSuccess(Arg.Do<Action<T>>(x => successCallbacks.Add(x))).Returns(promise);
            promise.Finally(Arg.Do<Action>(x => finallyCallbacks.Add(x))).Returns(promise);
            promise.OnFail(Arg.Do<Action<Exception>>(x => errorCallbacks.Add(x))).Returns(promise);

            return promise;
        }
    }
}