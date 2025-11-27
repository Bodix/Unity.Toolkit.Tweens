// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class BlendableMoveAnimation : AnimationTweenBehaviour
	{
		public Vector3 PositionDelta = new Vector3(0, 0, 1);
		public float Duration = 1;
		public CustomizableEase Ease;

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

			Tween tween = Transform.DOBlendableMoveBy(PositionDelta, Duration);
			if (Ease.IsCustom)
				tween.SetEase(Ease.CustomCurve);
			else tween.SetEase(Ease.Ease);

			return tween;
		}

		public Tween Play(Vector3 delta)
		{
			InitializeIfRequired();

			Tween tween = Transform.DOBlendableMoveBy(delta, Duration);
			if (Ease.IsCustom)
				tween.SetEase(Ease.CustomCurve);
			else tween.SetEase(Ease.Ease);

			return tween;
		}

		private void InitializeIfRequired()
		{
			if (!Transform && SameGameObjectWithTarget)
				_transform = transform;
		}
	}
}