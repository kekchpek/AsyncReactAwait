using System;
using AsyncReactAwait.Promises;
using UnityEngine;
using UnityEngine.UI;

namespace AwaitVirtualPromise
{
    public class AwaitableBehaviour : MonoBehaviour
    {
        
        private float _appearProgress;
        private bool _appearing;

        private IControllablePromise _appearPromise;

        public IPromise Appear()
        {
            Debug.Log("@@@ Appear Comp - start");
            if (_appearPromise != null)
            {
                return _appearPromise;
            }
            _appearing = true;
            _appearProgress = 0f;
            _appearPromise = new ControllablePromise();
            return _appearPromise;
        }

        private void Update()
        {
            if (_appearing)
            {
                _appearProgress += Time.deltaTime;
                if (_appearProgress > 1f)
                {
                    _appearing = false;
                    GetComponent<Image>().color = Color.red;
                    _appearPromise.Success();
                    Debug.Log("@@@ _appearPromise Comp - success");
                }
            }
        }
    }
}