using System;
using UniRx;
using UnityEngine;
using Zenject;
using static Utils.StringConstants;

public class CoinsController : IInitializable, IDisposable
{
    private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
    private DateTime _startOfSeason;
    private int _recursionCounter;

    public readonly ReactiveProperty<long> TotalCoins = new ReactiveProperty<long>();

    public void Initialize()
    {
        TotalCoins.Value = int.Parse(PlayerPrefs.GetString(CoinsKey, "0"));
    }

    public void AddCoins(long amount)
    {
        TotalCoins.Value += amount;
        PlayerPrefs.SetString(CoinsKey, TotalCoins.Value.ToString());
        PlayerPrefs.Save();
    }

    public void Dispose()
    {
        _compositeDisposable?.Dispose();
    }
}