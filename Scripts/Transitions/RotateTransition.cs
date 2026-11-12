// Copyright © 2026 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Transitions
{
	// Turns the element around its pivot from one angle to another.
	// An Elastic ease or a custom curve swings it, a Back ease spins it past the angle and back.
	public class RotateTransition : TweenTransition<Transform>
	{
		[Tooltip("Local Euler angles.")]
		public Vector3 TargetRotation = Vector3.zero;
		[Tooltip("Local Euler angles.")]
		public Vector3 InitialRotation = new Vector3(0, 0, -30);
		public float InDuration = 1;
		public float OutDuration = 1;
		public CustomizableEase InEase = new CustomizableEase(Ease.OutBack);
		public CustomizableEase OutEase = new CustomizableEase(Ease.InBack);

		public override Tween PlayIn()
		{
			return Play(InitialRotation, TargetRotation, InDuration, InEase);
		}

		public override Tween PlayOut()
		{
			return Play(TargetRotation, InitialRotation, OutDuration, OutEase);
		}

		private Tween Play(Vector3 from, Vector3 to, float duration, CustomizableEase ease)
		{
			InitializeIfRequired();

			// FastBeyond360 turns by the whole difference of the angles, so a turn of 360 degrees is not lost.
			return Target.DOLocalRotate(to, duration, RotateMode.FastBeyond360)
				.From(from)
				.SetEase(ease)
				.SetLink(Target.gameObject);
		}
	}
}
