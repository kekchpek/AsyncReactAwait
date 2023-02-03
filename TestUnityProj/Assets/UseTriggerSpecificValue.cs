using System;
using System.Threading;
using AsyncReactAwait.Promises;
using AsyncReactAwait.Trigger;
using AsyncReactAwait.Trigger.Extensions;
using UnityEngine;

public class UseTriggerSpecificValue : MonoBehaviour
{
    private IRegularTrigger<int> _trigger = new RegularTrigger<int>();

    private bool? _b;
    
    // Start is called before the first frame update
    private async void Start()
    {
        Thread t = new Thread(ParallelExecution);
        t.Start();
        await _trigger.WillBeEqual(9);
        transform.position += Vector3.up * 3f;
    }

    private void ParallelExecution()
    {
        Thread.Sleep(2000);
        _trigger.Trigger(8);
        Thread.Sleep(1000);
        _trigger.Trigger(9);
    }
}
