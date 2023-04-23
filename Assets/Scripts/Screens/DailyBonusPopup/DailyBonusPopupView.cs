using System;
using TMPro;
using UnityEngine;
using Utils;
using Button = UnityEngine.UI.Button;

namespace Screens.DailyBonusPopup
{
    public class DailyBonusPopupView : ScreenView
    {
        [SerializeField] private TextMeshProUGUI buttonLabel;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button emptySpaceButton;
        [SerializeField] private Button claimButton;

        private RectTransform _refillLivesTransform;
        private RectTransform _useLifeTransform;

        public event Action OnCloseScreen;
        public event Action OnClaimClick;

        private void Awake()
        {
            closeButton.ActionWithThrottle(() => OnCloseScreen?.Invoke());
            emptySpaceButton.ActionWithThrottle(() => OnCloseScreen?.Invoke());
            claimButton.ActionWithThrottle(() => OnClaimClick?.Invoke());
        }

        public void ShowCurrentBonus(long coins)
        {
            buttonLabel.text = $"Claim: {coins}";
        }
    }
}