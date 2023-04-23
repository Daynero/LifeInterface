using System;
using UniRx;
using UnityEngine.UI;
using Zenject;

namespace Utils
{
    public static class Extensions
    {
        public static void BindViewAndPresenter<TView, TPresenter>(this DiContainer container)
        {
            container.Bind<TView>().FromComponentInHierarchy().AsSingle();
            container.BindInterfacesAndSelfTo<TPresenter>().AsSingle().NonLazy();
        }

        public static void ActionWithThrottle(this Button button, Action action, int throttleMillis = 200)
        {
            button.OnClickAsObservable().ThrottleFirst(TimeSpan.FromMilliseconds(throttleMillis))
                .Subscribe(_ => { action?.Invoke(); }).AddTo(button);
        }
    }
}