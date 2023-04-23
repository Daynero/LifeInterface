using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static DG.Tweening.DOTween;
using Sequence = DG.Tweening.Sequence;

namespace Animations
{
    public class LivesPopupAnimation : MonoBehaviour
    {
        [SerializeField] private Transform contentTransform;
        [SerializeField] private Transform enterPointTransform;
        [SerializeField] private Transform exitPointTransform;
        [SerializeField] private Image bgImage;

        private Sequence _showAnimation;
        private Sequence _hideAnimation;
        private const float AppearSpeed = 0.7f;
        private Vector3 _middlePosition;

        public event Action OnAnimationCompleted;

        private void Start()
        {
            _middlePosition = contentTransform.localPosition;
        }

        public void Appear()
        {
            contentTransform.localPosition = enterPointTransform.localPosition;
            StopAnimation(_showAnimation);
            _showAnimation = Sequence()
                .Append(contentTransform.DOLocalMove(_middlePosition, AppearSpeed))
                .Join(bgImage.DOFade(0.5f, AppearSpeed))
                .SetEase(Ease.OutQuint);
        }

        public void Disappear()
        {
            
            StopAnimation(_hideAnimation);
            _hideAnimation = Sequence()
                .Append(contentTransform.DOLocalMove(exitPointTransform.localPosition, AppearSpeed))
                .Join(bgImage.DOFade(0, AppearSpeed))
                .SetEase(Ease.InQuint)
                .AppendCallback(() => OnAnimationCompleted?.Invoke());
        }

        private void StopAnimation(Sequence sequence)
        {
            if (sequence != null && sequence.IsActive())
            {
                sequence.Kill();
            }
        }
    }
}