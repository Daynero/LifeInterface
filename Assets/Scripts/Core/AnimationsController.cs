using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Zenject;

namespace Core
{
    public class AnimationsController : IInitializable
    {
        private static readonly Dictionary<Sequence, AnimationType> ActiveSequences = new Dictionary<Sequence, AnimationType>();
        private readonly global::Core.GameTime.GameTime _gameTime;
        private static bool _stopAddingAnimation;
        private static Action _allAnimationCompleted;

        public void Initialize()
        {
            _gameTime.PauseStatusChanged += isPause =>
            {
                if (isPause)
                {
                    foreach (var sequencePair in ActiveSequences.Where(sequencePair =>
                                 sequencePair.Value == AnimationType.Stopping || sequencePair.Value == AnimationType.Long))
                    {
                        sequencePair.Key.Pause();
                    }
                }
                else
                {
                    foreach (var sequencePair in ActiveSequences.Where(sequencePair =>
                                 sequencePair.Value == AnimationType.Stopping || sequencePair.Value == AnimationType.Long))
                    {
                        sequencePair.Key.Play();
                    }
                }
            };
        }

        public AnimationsController(global::Core.GameTime.GameTime gameTime)
        {
            _gameTime = gameTime;
        }

        public static void AddAndPlaySequence(Sequence sequence, AnimationType animationType)
        {
            if (sequence == null) return;

            sequence.OnKill(() => DeleteSequence(sequence));
            ActiveSequences.Add(sequence, animationType);
        }

        public static void StopAddingAnimations(Action animationsStopped)
        {
            _stopAddingAnimation = true;
            _allAnimationCompleted = animationsStopped;
            CheckStoppingAnimations();
        }

        private static void DeleteSequence(Sequence sequence)
        {
            if (!ActiveSequences.ContainsKey(sequence)) return;

            ActiveSequences.Remove(sequence);

            if (_stopAddingAnimation)
            {
                CheckStoppingAnimations();
            }
        }

        private static void CheckStoppingAnimations()
        {
            if (ActiveSequences.Count(valuePair => valuePair.Value is AnimationType.Stopping) != 0) return;
            
            foreach (var sequencePair in ActiveSequences.Where(sequencePair =>
                         sequencePair.Value is AnimationType.Long))
            {
                sequencePair.Key.Pause();
            }

            _allAnimationCompleted?.Invoke();
            _stopAddingAnimation = false;
            _allAnimationCompleted = null;
        }
    }
}