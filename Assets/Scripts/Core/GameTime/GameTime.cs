using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.GameTime
{
    public class GameTime : IInitializable, IDisposable
    {
        private bool _gameStarted;
        private float _seconds;
        private float _powerUpSeconds;
        private DateTime _timeWhenPauseActivate;
        private readonly ReactiveProperty<int> _totalSeconds = new ReactiveProperty<int>();
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private Action _onPowerUpTimerEnded;
        private float _activeTokenTime;
        private bool _isGameEnded;

        public ReactiveProperty<bool> Pause { get; } = new ReactiveProperty<bool>();
        public IReadOnlyReactiveProperty<int> TotalSeconds => _totalSeconds;

        public event Action<bool> PauseStatusChanged;

        public void Initialize()
        {
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(_compositeDisposable);
        }

        private void Update()
        {
            if (!_gameStarted || Pause.Value || _isGameEnded) return;

            _seconds += Time.deltaTime;
            if (_seconds > 1f)
            {
                _seconds--;
                _totalSeconds.Value++;
            }
        }

        public void AddTimeAction(TimeType timeActionType)
        {
            switch (timeActionType)
            {
                case TimeType.PauseStart:
                case TimeType.GameFinish:
                    Pause.Value = true;
                    break;
                case TimeType.PauseFinish:
                    Pause.Value = false;
                    break;
                case TimeType.GameStart:
                    _gameStarted = true;
                    break;
                default:
                    Pause.Value = Pause.Value;
                    break;
            }

            PauseStatusChanged?.Invoke(Pause.Value);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}