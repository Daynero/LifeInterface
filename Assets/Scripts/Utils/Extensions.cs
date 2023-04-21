using System;
using Core;
using DG.Tweening;
using Enums;
using UniRx;
using UnityEngine;
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

        public static void SetAlpha(this Image image, float alpha)
        {
            Color imageColor = image.color;
            image.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
        }

        public static Sequence AddToController(this Sequence sequence, AnimationType animationType = AnimationType.Stopping)
        {
            AnimationsController.AddAndPlaySequence(sequence, animationType);
            return sequence;
        }
    }
}