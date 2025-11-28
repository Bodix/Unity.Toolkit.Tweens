// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using Evolutex.Evolunity.Extensions;
using NaughtyAttributes;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class BlendableMoveAnimation : AnimationTweenBehaviour
	{
		public Vector3 PositionDelta = new Vector3(0, 0, 1);
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
			return Play(PositionDelta);
		}

		public Tween Play(Vector3 delta)
		{
			InitializeIfRequired();

			if (SplitEasingByAxes)
				return DOTween.Sequence()
					.Insert(0, Transform.DOBlendableMoveBy(delta.WithY(0).WithZ(0), Duration)
						.SetEase(XEase))
					.Insert(0, Transform.DOBlendableMoveBy(delta.WithX(0).WithZ(0), Duration)
						.SetEase(YEase))
					.Insert(0, Transform.DOBlendableMoveBy(delta.WithX(0).WithY(0), Duration)
						.SetEase(ZEase));
			else
				return Transform.DOBlendableMoveBy(delta, Duration)
					.SetEase(Ease);
		}

		private void InitializeIfRequired()
		{
			if (!Transform && SameGameObjectWithTarget)
				_transform = transform;
		}
	}
}