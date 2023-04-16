using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AwaitVirtualPromise
{
    public class AwaitableTaskBehaviour : MonoBehaviour
    {
        
        private float _appearProgress;
        private bool _appearing;

        private SemaphoreSlim _semaphore = new SemaphoreSlim(0, 1);
        private Task _appearTask;

        public Task Appear()
        {
            Debug.Log("@@@ Appear Comp - start");
            if (_appearTask != null)
            {
                return _appearTask;
            }
            _appearing = true;
            _appearProgress = 0f;
            _appearTask = new Task(() =>
            {
                _semaphore.Wait();
            });
            _appearTask.Start();
            return _appearTask;
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
                    _semaphore.Release();
                    Debug.Log("@@@ _appearPromise Comp - success");
                }
            }
        }
    }
}