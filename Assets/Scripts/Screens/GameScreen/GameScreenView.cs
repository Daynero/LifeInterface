using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Screens.GameScreen
{
    public class GameScreenView : MonoBehaviour
    {
        [SerializeField] private Button livesButton;
        [SerializeField] private TextMeshProUGUI lifeCount;
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private TextMeshProUGUI coinsCount;

        public event Action OnLivesClick;
        
        private void Awake()
        {
            livesButton.ActionWithThrottle(() => OnLivesClick?.Invoke());
        }

        public void UpdateTimer(string time)
        {
            timer.text = time;
        }
        
        public void UpdateLives(int lives)
        {
            lifeCount.text = lives.ToString();
        }

        public void UpdateCoinsCount(long coins)
        {
            coinsCount.text = $"Coins: {coins}";
        }
    }
}