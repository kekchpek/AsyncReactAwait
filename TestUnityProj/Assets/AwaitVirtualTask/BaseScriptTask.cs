using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AwaitVirtualPromise
{
    public abstract class BaseScriptTask : MonoBehaviour
    {

        private SemaphoreSlim _semaphore = new SemaphoreSlim(0, 1);

        [SerializeField] private Image _image;

        private float _maxTime = -1;
        private float _currentTime;

        protected virtual void Awake()
        {
            Debug.Log("@@@ Awake - start");
            ShowIntroInternal();
            Debug.Log("@@@ Awake - finish");
        }

        private async void ShowIntroInternal()
        {
            Debug.Log("@@@ ShowIntroInternal - start");
            await ShowIntro();
            Debug.Log("@@@ ShowIntroInternal - finish");
        }

        protected virtual async Task ShowIntro()
        {
            Debug.Log("@@@ ShowIntro - start");
            await Appear();
            Debug.Log("@@@ ShowIntro - finish");
        }

        private Task Appear()
        {
            var task = new Task(() =>
            {
                _semaphore.Wait();
            });
            task.Start();
            _maxTime = 2f;
            return task;
        }

        public void Update()
        {
            if (_maxTime > 0)
            {
                _currentTime += Time.deltaTime;
                if (_maxTime < _currentTime)
                {
                    _maxTime = -1f;
                    _image.color = Color.blue;
                    _semaphore.Release();
                }
            }
        }
    }
}
