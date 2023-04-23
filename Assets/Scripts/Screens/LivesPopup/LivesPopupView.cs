using System;
using Animations;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Button = UnityEngine.UI.Button;

namespace Screens.LivesPopup
{
    public class LivesPopupView : ScreenView
    {
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private TextMeshProUGUI lifeCount;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button emptySpaceButton;
        [SerializeField] private Button refillLivesButton;
        [SerializeField] private Button useLifeButton;
        [SerializeField] private RectTransform middlePositionTransform;
        [SerializeField] private VerticalLayoutGroup lifePanelLayoutGroup;
        
        private LivesPopupAnimation _livesPopupAnimation;
        private RectTransform _refillLivesTransform;
        private RectTransform _useLifeTransform;
        private Vector3 _refillLivesStartPosition;
        private Vector3 _useLifeStartPosition;
        private readonly Vector3 _refillButtonTargetScale = Vector3.one * 1.2f;
        private readonly Vector3 _useLifeButtonTargetScale = new Vector3(1.35f, 1.2f, 0);

        public event Action OnCloseScreen;
        public event Action OnUseLife;
        public event Action OnRefillLives;

        private void Awake()
        {
            closeButton.ActionWithThrottle(() => OnCloseScreen?.Invoke());
            emptySpaceButton.ActionWithThrottle(() => OnCloseScreen?.Invoke());
            refillLivesButton.ActionWithThrottle(() => OnRefillLives?.Invoke());
            useLifeButton.ActionWithThrottle(() => OnUseLife?.Invoke());

            _refillLivesTransform = refillLivesButton.gameObject.GetComponent<RectTransform>();
            _useLifeTransform = useLifeButton.gameObject.GetComponent<RectTransform>();
            _refillLivesStartPosition = _refillLivesTransform.localPosition;
            _useLifeStartPosition = _useLifeTransform.localPosition;
            _livesPopupAnimation = GetComponent<LivesPopupAnimation>();
        }

        public void UpdateTimer(string time)
        {
            timer.text = time;
        }
        
        public void UpdateLives(int lives)
        {
            lifeCount.text = lives.ToString();
        }

        public void UpdateUIState(LivesPopupState state)
        {
            switch (state)
            {
                case LivesPopupState.Default:
                    _refillLivesTransform.localPosition = _refillLivesStartPosition;
                    _useLifeTransform.localPosition = _useLifeStartPosition;
                    _refillLivesTransform.localScale = Vector3.one;
                    _useLifeTransform.localScale = Vector3.one;
                    break;
                case LivesPopupState.Empty:
                    _refillLivesTransform.localPosition = middlePositionTransform.localPosition;
                    _refillLivesTransform.localScale = _refillButtonTargetScale;
                    break;
                case LivesPopupState.Full:
                    _useLifeTransform.localPosition = middlePositionTransform.localPosition;
                    _useLifeTransform.localScale = _useLifeButtonTargetScale;
                    break;
            }

            lifePanelLayoutGroup.padding.top = state != LivesPopupState.Default ? 33 : 0;
            lifePanelLayoutGroup.padding.bottom = state == LivesPopupState.Empty ? -8 : 18;
            lifePanelLayoutGroup.SetLayoutVertical();
            _refillLivesTransform.gameObject.SetActive(state != LivesPopupState.Full);
            _useLifeTransform.gameObject.SetActive(state != LivesPopupState.Empty);
            timer.gameObject.SetActive(state != LivesPopupState.Full);
        }

        public void ShowAppearAnimation()
        {
            _livesPopupAnimation.Appear();
        }

        public void ShowDisappearAnimation(Action animationCompleted)
        {
            _livesPopupAnimation.Disappear();
            _livesPopupAnimation.OnAnimationCompleted += animationCompleted;
        }
    }
}