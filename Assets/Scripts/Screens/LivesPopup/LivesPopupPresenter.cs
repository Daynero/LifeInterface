using System;
using Enums;
using UniRx;
using Zenject;
using static Utils.GlobalConstants;

namespace Screens.LivesPopup
{
    public class LivesPopupPresenter : ScreenPresenter, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly LivesPopupView _view;
        private readonly ScreenNavigationSystem _screenNavigationSystem;
        private readonly LivesController _livesController;

        private readonly ReactiveProperty<LivesPopupState> _currentState =
            new ReactiveProperty<LivesPopupState>(LivesPopupState.Default);

        public LivesPopupPresenter(LivesPopupView view, LivesController livesController)
        {
            _view = view;
            _livesController = livesController;
        }

        public void Initialize()
        {
            _view.OnUseLife += UseLife;
            _view.OnRefillLives += RefillLives;

            _livesController.CurrentLivesObservable
                .Subscribe(lives =>
                {
                    _view.UpdateLives(lives);

                    _currentState.Value = lives switch
                    {
                        var x when x >= MaxLives => LivesPopupState.Full,
                        var x when x <= 0 => LivesPopupState.Empty,
                        _ => LivesPopupState.Default
                    };
                })
                .AddTo(_compositeDisposable);

            _livesController.TimeLeftObservable
                .Subscribe(time =>
                {
                    string formattedTimeLeft = _livesController.GetFormattedTimeLeft(time);
                    _view.UpdateTimer(formattedTimeLeft);
                })
                .AddTo(_compositeDisposable);
            _currentState.Subscribe(state => _view.UpdateUIState(state));
        }

        public override void ShowScreen(object extraData = null)
        {
            _view.OnCloseScreen += CloseScreen;
            _view.ShowAppearAnimation();
            _view.OpenCloseScreen(true);
        }

        private void RefillLives()
        {
            _livesController.AddLife(MaxLives);
        }

        private void UseLife()
        {
            _livesController.RemoveLife();
        }

        public override void CloseScreen()
        {
            _view.OnCloseScreen -= CloseScreen;
            _view.ShowDisappearAnimation(() =>
            {
                OnCloseAction.Invoke();
                _view.OpenCloseScreen(false);
            });
            
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}