using System.Threading;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using UnityEngine;

public class UseBindableSeveral : MonoBehaviour
{
    private IMutable<int> _counter = new Mutable<int>();
    private IBindable<int> Counter => _counter;

    private Thread _t;
    
    // Start is called before the first frame update
    private async void Start()
    {
        _counter.Value = 1;
        _t = new Thread(ParallelExecution);
        _t.Start();
        transform.position += Vector3.up * await Counter.WillBeEqual(3);
    }

    private void ParallelExecution()
    {
        Thread.Sleep(100);
        for (int i = 0; i < 5; i++)
        {
            _counter.Value = 1;
            _counter.Value = 3;
        }
    }

    private void OnDestroy()
    {
        _t.Abort();
    }
}
