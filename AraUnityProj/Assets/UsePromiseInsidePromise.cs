using System.Threading.Tasks;
using AsyncReactAwait.Promises;
using UnityEngine;

namespace DefaultNamespace
{
    public class UsePromiseInsidePromise : MonoBehaviour
    {
        
        private void Start()
        {
            DoWork1();
        }

        private async void DoWork1()
        {
            Debug.Log("Start work1");
            await DoWork2();
            Debug.Log("End work1");
            transform.position += Vector3.up * 3f;
        }

        private async IPromise DoWork2()
        {
            Debug.Log("Start work2");
            await Task.Delay(1);
            Debug.Log("End work2");
        }
    }
}