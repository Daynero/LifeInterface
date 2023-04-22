using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;
using static Utils.GlobalConstants;
using static Utils.StringConstants;

public class LivesController : IInitializable, IDisposable
{
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private readonly IntReactiveProperty _currentLives = new IntReactiveProperty(0);
    private readonly DoubleReactiveProperty _timeLeft = new DoubleReactiveProperty(NextLifeTime);
    private IEnumerator _timerCoroutine;

    public IObservable<int> CurrentLivesObservable => _currentLives;
    public IObservable<double> TimeLeftObservable => _timeLeft;

    public void Initialize()
    {
        _currentLives.Value = PlayerPrefs.GetInt(LivesKey, 0);

        _currentLives.Subscribe(lives =>
        {
            if (lives >= MaxLives)
            {
                _timeLeft.Value = 0;
            }
        }).AddTo(_compositeDisposable);
        
        _timerCoroutine = Timer();
        Observable.FromCoroutine(() => _timerCoroutine)
            .Subscribe()
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable?.Dispose();
    }

    private IEnumerator Timer()
    {
        while (true)
        {
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

    public void AddLife(int count = 1)
    {
        if (_currentLives.Value + count <= MaxLives)
        {
            _currentLives.Value += count;
        }
        else
        {
            _currentLives.Value = MaxLives;
        }
        
        PlayerPrefs.SetInt(LivesKey, _currentLives.Value);
    }

    public void RemoveLife()
    {
        if (_currentLives.Value >= MaxLives)
        {
            _timeLeft.Value = NextLifeTime;
        }
        
        _currentLives.Value = Mathf.Max(_currentLives.Value - 1, 0);
        PlayerPrefs.SetInt(LivesKey, _currentLives.Value);
    }
    
    public string GetFormattedTimeLeft(double time)
    {
        if (time == 0)
        {
            return "FULL";
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        return $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }

}