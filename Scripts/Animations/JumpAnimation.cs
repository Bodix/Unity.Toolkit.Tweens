// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using NaughtyAttributes;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class JumpAnimation : AnimationTweenBehaviour
	{
		public Vector3 TargetPosition = new Vector3(0, 0, 1);
		public float JumpPower = 1;
		public int NumberOfJumps = 1;
		public float Duration = 1;
		public CustomizableEase Ease = new CustomizableEase(DG.Tweening.Ease.Linear);
		[SerializeField]
		private bool SameGameObjectWithTarget = false;
		[SerializeField, HideIf(nameof(SameGameObjectWithTarget))]
		private Transform _transform;

		public Transform Transform => _transform;

		private void Awake()
		{
			InitializeIfRequired();
		}

		private void OnValidate()
		{
			InitializeIfRequired();
		}

		public override Tween Play()
		{
			return Play(TargetPosition);
		}

		public Tween Play(Vector3 targetPosition)
		{
			InitializeIfRequired();

			return Transform.DOJump(targetPosition, JumpPower, NumberOfJumps, Duration)
				.SetEase(Ease)
				.SetLink(Transform.gameObject);
		}

		private void InitializeIfRequired()
		{
			if (!Transform && SameGameObjectWithTarget)
				_transform = transform;
		}
	}
}