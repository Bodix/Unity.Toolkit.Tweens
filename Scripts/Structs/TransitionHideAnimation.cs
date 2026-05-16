using System;
using Bodix.Evolunity.Components;

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