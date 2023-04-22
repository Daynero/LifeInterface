using System;
using Core.GameTime;
using UniRx;
using Zenject;

namespace Screens.GamePausedPopup
{
    public class LivesPopupPresenter : ScreenPresenter, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly LivesPopupView _view;
        private readonly GameTime _gameTime;
        private readonly ScreenNavigationSystem _screenNavigationSystem;

        public LivesPopupPresenter(LivesPopupView view, GameTime gameTime)
        {
            _view = view;
            _gameTime = gameTime;
            
            Initialize();
        }

        public void Initialize()
        {
            _view.OnCloseScreen += CloseScreen;
            _view.OnUseLife += UseLife;
            _view.OnRefillLives += RefillLives;
        }

        private void RefillLives()
        {
            
        }

        private void UseLife()
        {
            
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
            _view.OpenCloseScreen(false);
        }
    }
}