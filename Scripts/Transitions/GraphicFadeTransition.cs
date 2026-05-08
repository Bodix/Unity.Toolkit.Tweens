// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using UnityEngine.UI;

namespace Toolkit.Tweens.Transitions
{
	public class GraphicFadeTransition : TweenTransition<Graphic>
	{
		public float TargetAlpha = 1;
		public float InitialAlpha = 0;
		public float InDuration = 1;
		public float OutDuration = 1;
		public Ease InEase = Ease.Linear;
		public Ease OutEase = Ease.Linear;

		public override Tween PlayIn()
		{
			InitializeIfRequired();

			return Target.DOFade(TargetAlpha, InDuration)
				.From(InitialAlpha)
				.SetEase(InEase)
				.SetLink(Target.gameObject);
		}

		public override Tween PlayOut()
		{
			InitializeIfRequired();

			return Target.DOFade(InitialAlpha, OutDuration)
				.From(TargetAlpha)
				.SetEase(OutEase)
				.SetLink(Target.gameObject);
		}
	}
}