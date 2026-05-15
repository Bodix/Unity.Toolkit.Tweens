using System;
using Evolutex.Evolunity.Components.Animations;

namespace Toolkit.Tweens
{
	public readonly struct TransitionShowAnimation : IAnimation
	{
		public TweenTransition Transition { get; }

		public TransitionShowAnimation(TweenTransition transition)
		{
			Transition = transition;
		}

		public void Play(Action onStart = null, Action onComplete = null)
		{
			Transition.PlayShow(onStart, onComplete);
		}
	}
}