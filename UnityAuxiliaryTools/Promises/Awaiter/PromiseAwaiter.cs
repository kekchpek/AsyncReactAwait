using System;
using System.Threading;
using UnityEngine;

namespace UnityAuxiliaryTools.Promises.Awaiter
{
    internal class PromiseAwaiter : BasePromiseAwaiter, IPromiseAwaiter
    {
        public PromiseAwaiter(IPromise promise, SynchronizationContext capturedContext)
            : base(promise, capturedContext)
        {
        }

        public void GetResult() { }
    }

    internal class PromiseAwaiter<T> : BasePromiseAwaiter, IPromiseAwaiter<T>
    {

        private readonly IPromise<T> _sourcePromise;

        public PromiseAwaiter(IPromise<T> promise, SynchronizationContext capturedContext)
            :base(promise, capturedContext)
        {
            _sourcePromise = promise;
        }

        public T GetResult()
        {
            if (_sourcePromise.TryGetResult(out var res))
            {
                return res;
            }
            throw new Exception("Can not obtain promise result!");
        }
    }
}
