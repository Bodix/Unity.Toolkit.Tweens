using System;
using Bodix.Evolunity.Components;

namespace Toolkit.Tweens
{
	[Serializable]
	public struct TwoTweenAnimations : IShowHideAnimations
	{
		public TweenAnimation showAnimation;
		public TweenAnimation hideAnimation;

		public IAnimation ShowAnimation => showAnimation;
		public IAnimation HideAnimation => hideAnimation;

		public void PlayShow(Action onStart = null, Action onComplete = null)
		{
			showAnimation.Play(onStart, onComplete);
		}

		public void PlayHide(Action onStart = null, Action onComplete = null)
		{
			hideAnimation.Play(onStart, onComplete);
		}
	}
}