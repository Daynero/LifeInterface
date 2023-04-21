using System.Collections;
using UnityEngine;

namespace Screens
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenView : MonoBehaviour
    {
        [SerializeField] private bool isDeactivateAfterAwake = true;
        private Coroutine _currentAnim;
        private CanvasGroup _canvasGroup;

        public void OpenCloseScreen(bool open)
        {
            if (_currentAnim != null)
            {
                StopCoroutine(_currentAnim);
            }

            if (open)
            {
                gameObject.SetActive(true);
                _currentAnim = StartCoroutine(OpenAnim());
            }
            else
            {
                _currentAnim = StartCoroutine(CloseAnim());
            }
        }

        protected void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected void Start()
        {
            gameObject.SetActive(!isDeactivateAfterAwake);
        }

        private IEnumerator OpenAnim()
        {
            float screenAlpha = 0f;
            _canvasGroup.alpha = screenAlpha;
            while (screenAlpha < 1f)
            {
                screenAlpha += Time.deltaTime / 0.2f;
                if (screenAlpha > 1f) screenAlpha = 1f;
                _canvasGroup.alpha = screenAlpha;
                yield return new WaitForFixedUpdate();
            }

            _canvasGroup.blocksRaycasts = true;
        }

        private IEnumerator CloseAnim()
        {
            _canvasGroup.blocksRaycasts = false;
            float screenAlpha = _canvasGroup.alpha;
            while (screenAlpha > 0f)
            {
                screenAlpha -= Time.deltaTime / 0.2f;
                if (screenAlpha < 0f) screenAlpha = 0f;
                _canvasGroup.alpha = screenAlpha;
                yield return new WaitForFixedUpdate();
            }

            gameObject.SetActive(false);
        }
    }
}