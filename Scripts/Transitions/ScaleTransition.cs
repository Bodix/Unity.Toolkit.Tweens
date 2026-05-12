// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens.Transitions
{
	public class ScaleTransition : TweenTransition<Transform>
	{
		public Vector3 TargetScale = Vector3.one;
		public Vector3 InitialScale = Vector3.zero;
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