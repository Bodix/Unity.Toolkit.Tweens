// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class RigidbodyRotateAnimation : TweenAnimation<Rigidbody>
	{
		public Vector3 TargetRotation = new Vector3(0, 180, 0);
		public float Duration = 1;
		public CustomizableEase Ease = new CustomizableEase(DG.Tweening.Ease.Linear);

		public override Tween Play()
		{
			return Play(TargetRotation);
		}

		public Tween Play(Vector3 targetPosition)
		{
			InitializeIfRequired();

			return Target.DORotate(targetPosition, Duration)
				.SetEase(Ease)
				.SetLink(Target.gameObject);
		}
	}
}