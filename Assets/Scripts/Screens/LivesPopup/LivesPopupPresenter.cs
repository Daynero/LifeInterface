using System;
using UniRx;
using Zenject;
using static Utils.GlobalConstants;

namespace Screens.GamePausedPopup
{
    public class LivesPopupPresenter : ScreenPresenter, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly LivesPopupView _view;
        private readonly ScreenNavigationSystem _screenNavigationSystem;
        private readonly LivesController _livesController;

        public LivesPopupPresenter(LivesPopupView view, LivesController livesController)
        {
            _view = view;
            _livesController = livesController;
        }

        public void Initialize()
        {
            _view.OnCloseScreen += CloseScreen;
            _view.OnUseLife += UseLife;
            _view.OnRefillLives += RefillLives;

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
        }

        private void RefillLives()
        {
            _livesController.AddLife(MaxLives);
        }

        private void UseLife()
        {
            _livesController.RemoveLife();
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }

        public override void ShowScreen(object extraData = null)
        {
            _view.OpenCloseScreen(true);
        }

        public override void CloseScreen()
        {
            OnCloseAction.Invoke();
            _view.OpenCloseScreen(false);
        }
    }
}