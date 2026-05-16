using System;
using Bodix.Evolunity.Components;

namespace Toolkit.Tweens
{
	[Serializable]
	public struct OneTweenTransition : IShowHideAnimations
	{
		public TweenTransition Transition;

		public IAnimation ShowAnimation => Transition.ShowAnimation;
		public IAnimation HideAnimation => Transition.HideAnimation;

		public void PlayShow(Action onStart = null, Action onComplete = null)
		{
			ShowAnimation.Play(onStart, onComplete);
		}

		public void PlayHide(Action onStart = null, Action onComplete = null)
		{
			HideAnimation.Play(onStart, onComplete);
		}
	}
}