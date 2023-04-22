using System;
using System.Collections;
using UniRx;
using Unity.VisualScripting;
using static Utils.GlobalConstants;

namespace Screens.GameScreen
{
    public class GameScreenPresenter : IInitializable, IDisposable
    {
        private readonly GameScreenView _view;
        private readonly ScreenNavigationSystem _screenNavigationSystem;
        private readonly LivesController _livesController;

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private IEnumerator _timerCoroutine;

        public GameScreenPresenter(GameScreenView view,
            ScreenNavigationSystem screenNavigationSystem, LivesController livesController)
        {
            _view = view;
            _screenNavigationSystem = screenNavigationSystem;
            _livesController = livesController;

            Initialize();
        }

        public void Initialize()
        {
            _view.OnLivesClick += OpenLivesPopup;

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

        private void OpenLivesPopup()
        {
            _screenNavigationSystem.Show(ScreenName.Lives);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}