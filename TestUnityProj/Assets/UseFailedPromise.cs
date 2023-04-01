using System;
using System.Threading;
using AsyncReactAwait.Promises;
using UnityEngine;

namespace DefaultNamespace
{
    public class UseFailedPromise : MonoBehaviour
    {
        private float _time;
        private IControllablePromise _movePromise;
    
        // Start is called before the first frame update
        private async void Start()
        {
            _movePromise = new ControllablePromise();
            try
            {
                await AwaitMovePromise(_movePromise);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
                transform.position += Vector3.up * 3f;
            }
        }

        private async IPromise AwaitMovePromise(IPromise p)
        {
            await p;
            throw new Exception("TestException");
        }

        private void FixedUpdate()
        {
            _time += Time.fixedDeltaTime;
            if (_time > 5f && !_movePromise.IsCompleted)
            {
                var thread = new Thread(() =>
                {
                    if (!_movePromise.IsCompleted)
                    {
                        _movePromise.Success();
                    }
                });
                thread.Start();
            }
        }
    }
}