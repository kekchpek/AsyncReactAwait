using System;
using System.Threading.Tasks;
using UnityAuxiliaryTools.UnityExecutor;
using UnityEngine;

namespace UnityAuxiliaryTools.Promises.Factory
{
    public class PromiseFactory : IPromiseFactory
    {
        private readonly IUnityExecutor _unityExecutor;

        public PromiseFactory(IUnityExecutor unityExecutor)
        {
            _unityExecutor = unityExecutor;
        }
        
        public IControllablePromise CreateFailedPromise(Exception exception)
        {
            var promise = new ControllablePromise(_unityExecutor);
            promise.Fail(exception);
            return promise;
        }

        public IControllablePromise<T> CreateFailedPromise<T>(Exception exception)
        {
            var promise = new ControllablePromise<T>(_unityExecutor);
            promise.Fail(exception);
            return promise;
        }

        public IControllablePromise CreateSucceedPromise()
        {
            var promise = new ControllablePromise(_unityExecutor);
            promise.Success();
            return promise;
        }

        public IControllablePromise<T> CreateSucceedPromise<T>(T result)
        {
            var promise = new ControllablePromise<T>(_unityExecutor);
            promise.Success(result);
            return promise;
        }

        public IControllablePromise CreatePromise()
        {
            return new ControllablePromise(_unityExecutor);
        }

        public IControllablePromise<T> CreatePromise<T>()
        {
            return new ControllablePromise<T>(_unityExecutor);
        }

        public IPromise CreateFromTask(Task task)
        {
            var promise = new ControllablePromise(_unityExecutor);
            task.ContinueWith(t =>
            {
                if (t.IsCanceled)
                    Debug.LogWarning("Promises doesn't support task canceling");
                if (t.Exception == null)
                    promise.Success();
                else
                {
                    promise.Fail(t.Exception.InnerException);
                }
            }, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.PreferFairness);
            return promise;
        }

        public IPromise<T> CreateFromTask<T>(Task<T> task)
        {
            var promise = new ControllablePromise<T>(_unityExecutor);
            task.ContinueWith(t =>
            {
                if (t.IsCanceled)
                    Debug.LogWarning("Promises doesn't support task canceling");
                if (t.Exception == null)
                    promise.Success(t.Result);
                else
                    promise.Fail(t.Exception.InnerException);
            }, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.PreferFairness);
            return promise;
        }

        public IPromise CreateFromPromise<T>(IPromise<T> promiseWithResult)
        {
            var promise = new ControllablePromise(_unityExecutor);
            promiseWithResult.OnSuccess(x => promise.Success());
            promiseWithResult.OnFail(e => promise.Fail(e));
            return promise;
        }
    }
}