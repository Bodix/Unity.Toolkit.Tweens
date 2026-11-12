// Copyright © 2026 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using Toolkit.Tweens.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Toolkit.Tweens.Transitions
{
	// Fills an image of the Filled type: a check mark draws itself, a bar grows.
	public class FillTransition : TweenTransition<Image>
	{
		public float TargetFill = 1;
		public float InitialFill = 0;
		public float InDuration = 1;
		public float OutDuration = 1;
		public CustomizableEase InEase = new CustomizableEase(Ease.OutCubic);
		public CustomizableEase OutEase = new CustomizableEase(Ease.InCubic);

		protected override void OnValidate()
		{
			base.OnValidate();

			if (Target && Target.type != Image.Type.Filled)
				Debug.LogWarning(nameof(FillTransition) + " needs an image of the Filled type.", this);
		}

		public override Tween PlayIn()
		{
			return Play(InitialFill, TargetFill, InDuration, InEase);
		}

		public override Tween PlayOut()
		{
			return Play(TargetFill, InitialFill, OutDuration, OutEase);
		}

		private Tween Play(float from, float to, float duration, CustomizableEase ease)
		{
			InitializeIfRequired();

			return Target.DOFillAmount(to, duration)
				.From(from)
				.SetEase(ease)
				.SetLink(Target.gameObject);
		}
	}
}
