// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class BlendableMoveAnimation : AnimationTweenBehaviour
	{
		public Vector3 PositionDelta = new Vector3(1, 1, 1);
		public float Duration = 1;
		public Ease XEase = Ease.Linear;
		public Ease YEase = Ease.Linear;
		public Ease ZEase = Ease.Linear;

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
			InitializeIfRequired();

			return Transform.DOBlendableMoveBy(PositionDelta, Duration)
				.SetEase(XEase);
		}

		public Tween Play(Vector3 delta)
		{
			InitializeIfRequired();

			return Transform.DOBlendableMoveBy(delta, Duration)
				.SetEase(XEase);
		}

		private void InitializeIfRequired()
		{
			if (!Transform && SameGameObjectWithTarget)
				_transform = transform;
		}
	}
}