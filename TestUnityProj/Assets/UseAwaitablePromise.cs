using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityAuxiliaryTools.Promises;
using UnityEngine;

public class UseAwaitablePromise : MonoBehaviour
{


    [AsyncMethodBuilder(typeof(bool))]
    private class TestClass : IAsyncResult, IAsyncDisposable
    {
        
        public TestAwaiter GetAwaiter()
        {
            return new TestAwaiter();
        }

        public object AsyncState { get; }
        public WaitHandle AsyncWaitHandle { get; }
        public bool CompletedSynchronously { get; }
        public bool IsCompleted { get; }
        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }

    private class TestAwaiter : INotifyCompletion, ICriticalNotifyCompletion
    {
        public bool IsCompleted { get; }

        public void GetResult() {}
        
        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }
    }

    private float _time;
    private IControllablePromise _movePromise;
    
    // Start is called before the first frame update
    private async void Start()
    {
        _movePromise = new ControllablePromise();
        await AwaitMovePromise(_movePromise);
        transform.position += Vector3.up * 2f;
    }

    private async IPromise AwaitMovePromise(IPromise p)
    {
        await p;
    }

    private void FixedUpdate()
    {
        _time += Time.fixedDeltaTime;
        if (_time > 5f && !_movePromise.IsCompleted)
        {
            var thread = new Thread(() =>
            {
                _movePromise.Success();
            });
            thread.Start();
        }
    }
}
