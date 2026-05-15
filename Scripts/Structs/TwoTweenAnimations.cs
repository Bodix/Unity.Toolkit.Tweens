using System;
using Evolutex.Evolunity.Components.Animations;
using Evolutex.Evolunity.Components.UI;
using UnityEngine;

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