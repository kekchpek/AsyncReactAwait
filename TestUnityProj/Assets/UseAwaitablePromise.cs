using System.Threading;
using UnityAuxiliaryTools.Promises;
using UnityEngine;

public class UseAwaitablePromise : MonoBehaviour
{

    private float _time;
    private IControllablePromise<Vector3> _movePromise;
    
    // Start is called before the first frame update
    private async void Start()
    {
        _movePromise = new ControllablePromise<Vector3>();
        transform.position += await _movePromise;
    }

    private void FixedUpdate()
    {
        _time += Time.fixedDeltaTime;
        if (_time > 5f && !_movePromise.IsCompleted)
        {
            var thread = new Thread(() =>
            {
                _movePromise.Success(Vector3.up);
            });
            thread.Start();
        }
    }
}
