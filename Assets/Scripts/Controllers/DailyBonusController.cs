using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;
using static Utils.GlobalConstants;
using static Utils.StringConstants;

namespace Controllers
{
    public class DailyBonusController : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private DateTime _startOfSeason;
        private int _currentSeason;
        private int _recursionCounter;
        private List<long> _dailyBonuses = new List<long>();

        public void Initialize()
        {
            (_startOfSeason, _currentSeason) = GetSeasonAndStartDate();
            var prevSeason = PlayerPrefs.GetInt(CurrentSeasonKey, _currentSeason);
            string dailyBonusListString = PlayerPrefs.GetString(DailyRewardsKey, "");

            if (prevSeason != _currentSeason || String.IsNullOrEmpty(dailyBonusListString))
            {
                FillDailyBonuses();
            }
            else
            {
                _dailyBonuses = dailyBonusListString.Split(',').Select(long.Parse).ToList();
            }
        }

        private void FillDailyBonuses()
        {
            int daysInSeason = MaxDaysInSeason;

            for (int i = 0; i < daysInSeason; i++)
            {
                if (i == 0)
                {
                    _dailyBonuses.Add(DailyCoinsForFirstDay);
                }
                else if (i == 1)
                {
                    _dailyBonuses.Add(DailyCoinsForSecondDay);
                }
                else
                {
                    long previousDayCoins = (long) Math.Ceiling(_dailyBonuses[i - 1] * DailyPercentOfPrevDay);
                    long twoDaysAgoCoins = _dailyBonuses[i - 2];
                    long coins = previousDayCoins + twoDaysAgoCoins;
                    _dailyBonuses.Add(coins);
                }
            }

            string dailyBonusListString = string.Join(",", _dailyBonuses.Select(l => l.ToString()).ToArray());
            PlayerPrefs.SetString(DailyRewardsKey, dailyBonusListString);
        }

        public long GetDailyBonus()
        {
            var daysSinceStartOfSeason = (int) DateTime.Today.Subtract(_startOfSeason).TotalDays;
            return _dailyBonuses[Math.Min(daysSinceStartOfSeason, _dailyBonuses.Count - 1)];
        }

        private (DateTime, int) GetSeasonAndStartDate()
        {
            var today = DateTime.Today;

            if (today.Month >= 9)
                return (new DateTime(today.Year, 9, 1), 3);

            if (today.Month >= 6)
                return (new DateTime(today.Year, 6, 1), 2);

            if (today.Month >= 3)
                return (new DateTime(today.Year, 3, 1), 1);

            return (new DateTime(today.Year - 1, 12, 1), 4);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}