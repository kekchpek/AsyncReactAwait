using System;
using System.Threading;

namespace AsyncReactAwait.Promises.Awaiter
{
    internal class PromiseAwaiter : BasePromiseAwaiter<IPromiseAwaiter>, IPromiseAwaiter
    {

        private IPromise _sourcePromise;

        public PromiseAwaiter(IPromise promise, SynchronizationContext capturedContext)
            : base(promise, capturedContext)
        {
            _sourcePromise = promise;
        }

        public override IPromiseAwaiter GetAwaiter()
        {
            return this;
        }

        public void GetResult() 
        {
            _sourcePromise.ThrowIfFailed();
        }
    }

    internal class PromiseAwaiter<T> : BasePromiseAwaiter<IPromiseAwaiter<T>>, IPromiseAwaiter<T>
    {

        private readonly IPromise<T> _sourcePromise;

        public PromiseAwaiter(IPromise<T> promise, SynchronizationContext capturedContext)
            :base(promise, capturedContext)
        {
            _sourcePromise = promise;
        }

        public override IPromiseAwaiter<T> GetAwaiter()
        {
            return this;
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
