using System.Threading;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using UnityEngine;

public class UseBindable : MonoBehaviour
{
    private IMutable<int> _counter = new Mutable<int>();
    private IBindable<int> Counter => _counter;

    private Thread _t;
    
    // Start is called before the first frame update
    private async void Start()
    {
        _counter.Value = 10000000;
        _t = new Thread(ParallelExecution);
        _t.Start();
        transform.position += Vector3.up * await Counter.WillBeEqual(3);
    }

    private void ParallelExecution()
    {
        Thread.Sleep(100);
        while (_counter.Value > -100)
        {
            _counter.Value--;
        }
    }

    private void OnDestroy()
    {
        _t.Abort();
    }
}
