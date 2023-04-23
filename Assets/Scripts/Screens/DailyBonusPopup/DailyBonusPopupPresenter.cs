using System;
using Controllers;
using UniRx;
using UnityEngine;
using Utils;
using Zenject;

namespace Screens.DailyBonusPopup
{
    public class DailyBonusPopupPresenter : ScreenPresenter, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly DailyBonusPopupView _view;
        private readonly CoinsController _coinsController;
        private readonly DailyBonusController _dailyBonusController;
        private long _currentDailyBonus;

        public DailyBonusPopupPresenter(DailyBonusPopupView view, CoinsController coinsController,
            DailyBonusController dailyBonusController)
        {
            _view = view;
            _coinsController = coinsController;
            _dailyBonusController = dailyBonusController;
        }

        public void Initialize()
        {
            _view.OnCloseScreen += CloseScreen;
            _view.OnClaimClick += ClaimClick;
        }

        public override void ShowScreen(object extraData = null)
        {
            _view.OpenCloseScreen(true);
            _currentDailyBonus = _dailyBonusController.GetDailyBonus();
            _view.ShowCurrentBonus(_currentDailyBonus);
        }

        private void ClaimClick()
        {
            _coinsController.AddCoins(_currentDailyBonus);
            PlayerPrefs.SetInt(StringConstants.LastDayRewardKey, DateTime.Now.Day);
            CloseScreen();
        }

        public override void CloseScreen()
        {
            OnCloseAction.Invoke();
            _view.OpenCloseScreen(false);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}