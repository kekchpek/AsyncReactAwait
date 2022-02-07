using System;
using System.Threading.Tasks;

namespace UnityAuxiliaryTools.Promises.Factory
{
    public interface IPromiseFactory
    {
        IControllablePromise CreateFailedPromise(Exception exception);
        IControllablePromise<T> CreateFailedPromise<T>(Exception exception);

        IControllablePromise CreateSucceedPromise();
        IControllablePromise<T> CreateSucceedPromise<T>(T result);

        IControllablePromise CreatePromise();
        IControllablePromise<T> CreatePromise<T>();

        IPromise CreateFromTask(Task task);
        IPromise<T> CreateFromTask<T>(Task<T> task);

        IPromise CreateFromPromise<T>(IPromise<T> promiseWithResult);
    }
}