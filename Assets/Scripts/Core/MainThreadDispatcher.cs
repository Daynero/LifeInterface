using System.Collections;
using UnityEngine;

namespace Core
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        public void StartExternalCoroutine(IEnumerator coroutine)
        {
            if(coroutine == null) return;
            StartCoroutine(coroutine);
        }

        public void StopExternalCoroutine(IEnumerator coroutine)
        {
            if(coroutine == null) return;
            StopCoroutine(coroutine);
        }
    }
}