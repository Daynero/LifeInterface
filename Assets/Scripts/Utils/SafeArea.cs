using UnityEngine;

namespace Utils
{
    public class SafeArea : MonoBehaviour
    {
        private RectTransform _panel;
        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);

        private void Awake()
        {
            _panel = GetComponent<RectTransform>();
            Refresh();
        }

        private void Refresh()
        {
            Rect safeArea = Screen.safeArea;
            if (safeArea != _lastSafeArea)
            {
                ApplySafeArea(safeArea);
            }
        }

        private void ApplySafeArea(Rect r)
        {
            _lastSafeArea = r;

            float kW = (float) 1125 / Screen.width;
            float kH = (float) 2436 / Screen.height;
            Rect rect = _panel.rect;
            rect.x = r.x * kW;
            rect.y = r.y * kH;
            rect.width = r.width * kW;
            rect.height = r.height * kH;
            _panel.offsetMin = new Vector2(rect.x, rect.y);
            _panel.offsetMax = new Vector2(rect.width + rect.x - 1125, rect.height + rect.y - 2436);

            Camera cam = Camera.main;
            const float origScaleFactor = 2436 / 1125;
            float factScaleFactor = r.height / r.width;
            cam!.orthographicSize = (12f / origScaleFactor) * ((float) Screen.height / Screen.width);
            var camTransform = cam.transform;
            Vector3 camPos = camTransform.position;
            camPos.y = -2 + (origScaleFactor / factScaleFactor - 1) * 9.3f;
            camTransform.position = camPos;

            var panelRect = _panel.rect;
            Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
            Debug.LogFormat("My safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
                name, panelRect.x, panelRect.y, panelRect.width, panelRect.height, Screen.width, Screen.height);
        }
    }
}