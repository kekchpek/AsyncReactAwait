using System.Threading;
using UnityAuxiliaryTools.Promises;
using UnityEngine;

public class UseAwaitablePromise : MonoBehaviour
{

    private float _time;
    private IControllablePromise _movePromise;
    
    // Start is called before the first frame update
    private async void Start()
    {
        _movePromise = new ControllablePromise();
        await _movePromise;
        transform.position += Vector3.up;
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
