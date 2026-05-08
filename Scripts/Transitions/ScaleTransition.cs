using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens.Transitions
{
	public class ScaleTransition : TweenTransition<Transform>
	{
		public Vector3 TargetScale = new Vector3(1, 1, 1);
		public Vector3 InitialScale = new Vector3(0, 0, 0);
		public float InDuration = 1;
		public float OutDuration = 1;
		public Ease InEase = Ease.Linear;
		public Ease OutEase = Ease.Linear;

		public override Tween PlayIn()
		{
			InitializeIfRequired();

			return Target.DOScale(TargetScale, InDuration)
				.From(InitialScale)
				.SetEase(InEase)
				.SetLink(Target.gameObject);
		}

		public override Tween PlayOut()
		{
			InitializeIfRequired();

			return Target.DOScale(InitialScale, OutDuration)
				.From(TargetScale)
				.SetEase(OutEase)
				.SetLink(Target.gameObject);
		}
	}
}