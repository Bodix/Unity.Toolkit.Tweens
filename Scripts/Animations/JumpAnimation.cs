// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using NaughtyAttributes;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class JumpAnimation : TweenAnimation<Transform>
	{
		public Vector3 TargetPosition = new Vector3(0, 0, 1);
		public float JumpPower = 1;
		public int NumberOfJumps = 1;
		public float Duration = 1;
		public CustomizableEase Ease = new CustomizableEase(DG.Tweening.Ease.Linear);

		public override Tween Play()
		{
			return Play(TargetPosition);
		}

		public Tween Play(Vector3 targetPosition)
		{
			InitializeIfRequired();

			return Target.DOJump(targetPosition, JumpPower, NumberOfJumps, Duration)
				.SetEase(Ease)
				.SetLink(Target.gameObject);
		}
	}
}