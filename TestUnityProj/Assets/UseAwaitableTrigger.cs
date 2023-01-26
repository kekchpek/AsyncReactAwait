using System.Threading;
using UnityAuxiliaryTools.Promises;
using UnityAuxiliaryTools.Trigger;
using UnityEngine;

public class UseAwaitableTrigger : MonoBehaviour
{

    private float _time;
    private bool _done;
    private IRegularTrigger<Vector3> _moveTrigger;
    
    // Start is called before the first frame update
    private async void Start()
    {
        _moveTrigger = new RegularTrigger<Vector3>();
        transform.position += await AwaitTrigger(_moveTrigger);
    }

    private async IPromise<Vector3> AwaitTrigger(IRegularTrigger<Vector3> t)
    {
        return await t;
    }

    private void FixedUpdate()
    {
        _time += Time.fixedDeltaTime;
        if (_time > 2f && !_done)
        {
            _done = true;
            var thread = new Thread(() =>
            {
                _moveTrigger.Trigger(Vector3.up);
            });
            thread.Start();
        }
    }
}
