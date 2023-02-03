using AsyncReactAwait.Promises;
using UnityEngine;

public class UseAwaitablePromiseInstantly : MonoBehaviour
{
    private float _time;
    private IControllablePromise _movePromise;
    
    // Start is called before the first frame update
    private async void Start()
    {
        _movePromise = new ControllablePromise();
        _movePromise.Success();
        await _movePromise;
        transform.position += Vector3.up * 3f;
    }
}
