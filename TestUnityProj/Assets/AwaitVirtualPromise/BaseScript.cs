using AsyncReactAwait.Promises;
using UnityEngine;
using UnityEngine.UI;

namespace AwaitVirtualPromise
{
    public abstract class BaseScript : MonoBehaviour
    {

        private IControllablePromise _controllablePromise = new ControllablePromise();

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

        protected virtual async IPromise ShowIntro()
        {
            Debug.Log("@@@ ShowIntro - start");
            await Appear();
            Debug.Log("@@@ ShowIntro - finish");
        }

        private IPromise Appear()
        {
            _controllablePromise = new ControllablePromise();
            _maxTime = 2f;
            return _controllablePromise;
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
                    _controllablePromise.Success();
                }
            }
        }
    }
}
