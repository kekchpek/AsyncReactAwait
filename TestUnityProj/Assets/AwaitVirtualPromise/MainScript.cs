using AsyncReactAwait.Promises;
using UnityEngine;
using UnityEngine.UI;

namespace AwaitVirtualPromise
{
    public class MainScript : BaseScript
    {

        [SerializeField] private Image _image2;
        [SerializeField] private AwaitableBehaviour _awaitable;

        protected override void Awake()
        {
            Debug.Log("@@@ Awake Child - start");
            base.Awake();
            _image2.color = Color.yellow;
            Debug.Log("@@@ Awake Child - finish");
        }
        
        protected override async IPromise ShowIntro()
        {
            Debug.Log("@@@ ShowIntro Child - start");
            await base.ShowIntro();
            Debug.Log("@@@ ShowIntro Child - 1");
            await _awaitable.Appear();
            Debug.Log("@@@ ShowIntro Child - 2");
            _image2.color = Color.green;
            Debug.Log("@@@ ShowIntro Child - finish");
        }
    }
}