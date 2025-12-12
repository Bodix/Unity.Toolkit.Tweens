// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using NaughtyAttributes;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class RigidbodyMoveAnimation : AnimationTweenBehaviour
	{
		public Vector3 TargetPosition = new Vector3(0, 0, 1);
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

			if (SplitEasingByAxes)
				return DOTween.Sequence()
					.Insert(0, Rigidbody.DOMoveX(targetPosition.x, Duration)
						.SetEase(XEase))
					.Insert(0, Rigidbody.DOMoveY(targetPosition.y, Duration)
						.SetEase(YEase))
					.Insert(0, Rigidbody.DOMoveZ(targetPosition.z, Duration)
						.SetEase(ZEase))
					.SetLink(Rigidbody.gameObject);
			else
				return Rigidbody.DOMove(targetPosition, Duration)
					.SetEase(Ease)
					.SetLink(Rigidbody.gameObject);
		}

		private void InitializeIfRequired()
		{
			if (!Rigidbody && SameGameObjectWithTarget)
				_rigidbody = GetComponent<Rigidbody>();
		}
	}
}