using UnityEngine;

namespace Screens
{
    public class ScreenView : MonoBehaviour
    {
        [SerializeField] private bool isDeactivateAfterAwake = true;

        public void OpenCloseScreen(bool open)
        {
            if (open)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected void Start()
        {
            gameObject.SetActive(!isDeactivateAfterAwake);
        }
    }
}