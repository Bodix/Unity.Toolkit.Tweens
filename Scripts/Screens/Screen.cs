using DG.Tweening;
using Toolkit.Tweens.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Toolkit.Tweens.Screens
{
	public class Screen : AbstractAnimatedScreen
	{
		[SerializeField, FormerlySerializedAs("_tweenBehaviour")]
		private TweenTransition _transition;

		public override void Show()
		{
			gameObject.SetActive(true);
		}

		public override void Hide()
		{
			gameObject.SetActive(false);
		}

		public override Tween ShowTween => _transition.PlayIn().AddOnStart(Show);
		public override Tween HideTween => _transition.PlayOut().AddOnComplete(Hide);
	}
}