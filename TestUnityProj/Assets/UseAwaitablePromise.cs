using System.Threading;
using AsyncReactAwait.Promises;
using UnityEngine;

public class UseAwaitablePromise : MonoBehaviour
{
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
