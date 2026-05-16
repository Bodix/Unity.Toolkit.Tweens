// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using Bodix.Evolunity.Extensions;
using DG.Tweening;
using NaughtyAttributes;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class BlendableMoveAnimation : TweenAnimation<Transform>
	{
		public Vector3 PositionDelta = Vector3.forward;
		public float Duration = 1;
		public bool SplitEasingByAxes = false;
		[HideIf(nameof(SplitEasingByAxes))]
		public CustomizableEase Ease = new CustomizableEase(DG.Tweening.Ease.Linear);
		[ShowIf(nameof(SplitEasingByAxes))]
		public CustomizableEase XEase = new CustomizableEase(DG.Tweening.Ease.Linear);
		[ShowIf(nameof(SplitEasingByAxes))]
		public CustomizableEase YEase = new CustomizableEase(DG.Tweening.Ease.Linear);
		[ShowIf(nameof(SplitEasingByAxes))]
		public CustomizableEase ZEase = new CustomizableEase(DG.Tweening.Ease.Linear);

		public override Tween Play()
		{
			return Play(PositionDelta);
		}

		public Tween Play(Vector3 delta)
		{
			InitializeIfRequired();

			if (SplitEasingByAxes)
				return DOTween.Sequence()
					.Insert(0, Target.DOBlendableMoveBy(delta.WithY(0).WithZ(0), Duration)
						.SetEase(XEase))
					.Insert(0, Target.DOBlendableMoveBy(delta.WithX(0).WithZ(0), Duration)
						.SetEase(YEase))
					.Insert(0, Target.DOBlendableMoveBy(delta.WithX(0).WithY(0), Duration)
						.SetEase(ZEase))
					.SetLink(Target.gameObject);
			else
				return Target.DOBlendableMoveBy(delta, Duration)
					.SetEase(Ease)
					.SetLink(Target.gameObject);
		}
	}
}