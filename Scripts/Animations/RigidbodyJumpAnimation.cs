// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.DOTweenEditor;
using DG.Tweening;
using NaughtyAttributes;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class RigidbodyJumpAnimation : AnimationTweenBehaviour
	{
		[ValidateInput(nameof(IsRigidbodyNotKinematic), "This component works only with `Rigidbody.isKinematic == false`")]
		public Vector3 TargetPosition = new Vector3(0, 0, 1);
		public float JumpPower = 1;
		public int NumberOfJumps = 1;
		public float Duration = 1;
		public CustomizableEase Ease = new CustomizableEase(DG.Tweening.Ease.Linear);
		[SerializeField]
		private bool SameGameObjectWithTarget = false;
		[SerializeField, HideIf(nameof(SameGameObjectWithTarget))]
		private Rigidbody _rigidbody;

		public Rigidbody Rigidbody => _rigidbody;

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

			if (IsRigidbodyNotKinematic())
				return Rigidbody.DOJump(targetPosition, JumpPower, NumberOfJumps, Duration)
					.SetEase(Ease)
					.SetLink(Rigidbody.gameObject);
			else
			{
				Debug.LogError("Playing Rigidbody jump animation is not possible when Rigidbody is kinematic. " +
					"Playing empty tween instead");

				return DOVirtual.DelayedCall(Duration, null);
			}
		}

		private void InitializeIfRequired()
		{
			if (!Rigidbody && SameGameObjectWithTarget)
				_rigidbody = GetComponent<Rigidbody>();
		}

		private bool IsRigidbodyNotKinematic()
		{
			return Rigidbody && !Rigidbody.isKinematic;
		}
	}
}