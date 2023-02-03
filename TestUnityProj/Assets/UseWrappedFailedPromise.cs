using System;
using AsyncReactAwait.Promises;
using UnityEngine;

public class UseWrappedFailedPromise : MonoBehaviour
{
    private IControllablePromise _movePromise;
    
    // Start is called before the first frame update
    private async void Start()
    {
        try
        {
            await Await();
            transform.position -= Vector3.up * 3f;
        }
        catch (Exception e)
        {
            transform.position += Vector3.up * 3f;
        }
    }

    private async IPromise Await()
    {
        _movePromise = new ControllablePromise();
        _movePromise.Fail(new Exception("Test exception!"));
        await _movePromise;
    }
}
