using AsyncReactAwait.Promises;
using UnityEngine;

public class UseWrappedAwaitablePromiseInstantly : MonoBehaviour
{
    private float _time;
    private IControllablePromise _movePromise;
    
    // Start is called before the first frame update
    private async void Start()
    {
        await Await();
        transform.position += Vector3.up * 3f;
    }

    private async IPromise Await()
    {
        _movePromise = new ControllablePromise();
        _movePromise.Success();
        await _movePromise;
    }
}
