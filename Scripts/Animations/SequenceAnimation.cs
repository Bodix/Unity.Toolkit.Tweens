// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using Evolutex.Evolunity.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
    public class SequenceAnimation : TransitionTweenBehaviour
    {
        [SerializeField]
        private TransitionTweenBehaviour[] _tweenBehaviours;

        public override Tween PlayIn()
        {
            Sequence sequence = DOTween.Sequence();

            foreach (TransitionTweenBehaviour behaviour in _tweenBehaviours)
                sequence.Append(behaviour.PlayIn());

            return sequence;
        }

        public override Tween PlayOut()
        {
            Sequence sequence = DOTween.Sequence();

            // In reversed order.
            foreach (TransitionTweenBehaviour behaviour in _tweenBehaviours.Reverse())
                sequence.Append(behaviour.PlayOut());

            return sequence;
        }
    }
}