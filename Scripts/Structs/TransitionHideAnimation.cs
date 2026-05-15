using System;
using Evolutex.Evolunity.Components.Animations;

namespace Toolkit.Tweens
{
	public readonly struct TransitionHideAnimation : IAnimation
	{
		public TweenTransition Transition { get; }

		public TransitionHideAnimation(TweenTransition transition)
		{
			Transition = transition;
		}

		public void Play(Action onStart = null, Action onComplete = null)
		{
			Transition.PlayHide(onStart, onComplete);
		}
	}
}