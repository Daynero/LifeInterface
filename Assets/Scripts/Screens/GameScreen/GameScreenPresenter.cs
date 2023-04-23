using System;
using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Screens.GameScreen
{
    public class GameScreenPresenter : IInitializable, IDisposable
    {
        private readonly GameScreenView _view;
        private readonly ScreenNavigationSystem _screenNavigationSystem;
        private readonly LivesController _livesController;
        private readonly CoinsController _coinsController;

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private IEnumerator _timerCoroutine;

        public GameScreenPresenter(GameScreenView view,
            ScreenNavigationSystem screenNavigationSystem, LivesController livesController,
            CoinsController coinsController)
        {
            _view = view;
            _screenNavigationSystem = screenNavigationSystem;
            _livesController = livesController;
            _coinsController = coinsController;

            Initialize();
        }

        public void Initialize()
        {
            _view.OnLivesClick += OpenLivesPopup;

            CheckCurrentDayAndOpenDaily();

            _livesController.CurrentLivesObservable
                .Subscribe(lives => _view.UpdateLives(lives))
                .AddTo(_compositeDisposable);

            _livesController.TimeLeftObservable
                .Subscribe(time =>
                {
                    string formattedTimeLeft = _livesController.GetFormattedTimeLeft(time);
                    _view.UpdateTimer(formattedTimeLeft);
                })
                .AddTo(_compositeDisposable);

            _coinsController.TotalCoins.Subscribe(coins => _view.UpdateCoinsCount(coins)).AddTo(_compositeDisposable);
        }

        private void CheckCurrentDayAndOpenDaily()
        {
            int prevRewardsDay = PlayerPrefs.GetInt(StringConstants.LastDayRewardKey, -1);
            int currentDay = DateTime.Now.Day;
            
            if (prevRewardsDay != currentDay)
            {
                Observable.FromCoroutine(OpenDailyBonusAfterScreenShow).Subscribe().AddTo(_compositeDisposable);
            }
        }

        private void OpenLivesPopup()
        {
            _screenNavigationSystem.Show(ScreenName.Lives);
        }

        private IEnumerator OpenDailyBonusAfterScreenShow()
        {
            yield return WaitForScreenToShow();
            _screenNavigationSystem.Show(ScreenName.DailyBonus);
        }

        private IEnumerator WaitForScreenToShow()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => _view.gameObject.activeInHierarchy);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}