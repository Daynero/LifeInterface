using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Screens.GamePausedPopup
{
    public class LivesPopupView : ScreenView
    {
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private TextMeshProUGUI lifeCount;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button emptySpaceButton;
        [SerializeField] private Button refillLivesButton;
        [SerializeField] private Button useLifeButton;

        public event Action OnCloseScreen;
        public event Action OnUseLife;
        public event Action OnRefillLives;

        private new void Awake()
        {
            base.Awake();
            closeButton.ActionWithThrottle(() => OnCloseScreen?.Invoke());
            emptySpaceButton.ActionWithThrottle(() => OnCloseScreen?.Invoke());
            refillLivesButton.ActionWithThrottle(() => OnRefillLives?.Invoke());
            useLifeButton.ActionWithThrottle(() => OnUseLife?.Invoke());
        }
        
        public void UpdateTimer(string time)
        {
            timer.text = time;
        }
        
        public void UpdateLives(int lives)
        {
            lifeCount.text = lives.ToString();
        }
    }
}