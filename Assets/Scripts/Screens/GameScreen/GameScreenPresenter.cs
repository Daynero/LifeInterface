using System;
using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using static Utils.GlobalConstants;
using MainThreadDispatcher = Core.MainThreadDispatcher;

namespace Screens.GameScreen
{
    public class GameScreenPresenter : IInitializable, IDisposable
    {
        private readonly GameScreenView _view;
        private readonly ScreenNavigationSystem _screenNavigationSystem;
        private readonly MainThreadDispatcher _mainThreadDispatcher;
        
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly IntReactiveProperty _currentLives = new IntReactiveProperty(0);
        private readonly DoubleReactiveProperty _timeLeft = new DoubleReactiveProperty(NextLifeTime);
        private IEnumerator _timerCoroutine;

        public GameScreenPresenter(GameScreenView view,
            ScreenNavigationSystem screenNavigationSystem, MainThreadDispatcher mainThreadDispatcher)
        {
            _view = view;
            _screenNavigationSystem = screenNavigationSystem;
            _mainThreadDispatcher = mainThreadDispatcher;

            Initialize();
        }

        public void Initialize()
        {
            _view.OnLivesClick += OpenLivesPopup;
        
            _currentLives.Subscribe(lives =>
            {
                _view.UpdateLives(lives);
            }).AddTo(_compositeDisposable);
            
            _timeLeft.Subscribe(time =>
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(time);
                _view.UpdateTimer($"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}");
            }).AddTo(_compositeDisposable);

            _mainThreadDispatcher.StopExternalCoroutine(_timerCoroutine);
            _timerCoroutine = Timer();
            _mainThreadDispatcher.StartExternalCoroutine(_timerCoroutine);
        }
        
        private IEnumerator Timer() {
            while (true) {
                yield return new WaitForSeconds(1.0f);

                if (_currentLives.Value < MaxLives)
                {
                    _timeLeft.Value -= 1;

                    if (_timeLeft.Value <= 0)
                    {
                        _currentLives.Value += 1;

                        _timeLeft.Value = _currentLives.Value >= MaxLives ? 0 : NextLifeTime;
                    }
                }
            }
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