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
		[ValidateInput(nameof(IsRigidbodyNotKinematic), nameof(SplitEasingByAxes) + " works only with `Rigidbody.isKinematic == false`")]
		public bool SplitEasingByAxes = false;
		[HideIf(nameof(IsSplitEasingByAxesWorks))]
		public CustomizableEase Ease = new CustomizableEase(DG.Tweening.Ease.Linear);
		[ShowIf(nameof(IsSplitEasingByAxesWorks))]
		public CustomizableEase XEase = new CustomizableEase(DG.Tweening.Ease.Linear);
		[ShowIf(nameof(IsSplitEasingByAxesWorks))]
		public CustomizableEase YEase = new CustomizableEase(DG.Tweening.Ease.Linear);
		[ShowIf(nameof(IsSplitEasingByAxesWorks))]
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
			ValidateSplitEasingByAxes();

			if (IsSplitEasingByAxesWorks())
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

		private void ValidateSplitEasingByAxes()
		{
			if (SplitEasingByAxes && !IsSplitEasingByAxesWorks())
				Debug.LogError("Trying to split easing by axes when it's not possible " +
					"because Rigidbody is kinematic. The split was not carried out");
		}

		private bool IsRigidbodyNotKinematic()
		{
			return Rigidbody && !Rigidbody.isKinematic;
		}

		/// <summary>
		/// DOMoveX, DOMoveY and DOMoveZ methods will "lock" other axes by the tween.
		/// <see href="https://dotween.demigiant.com/documentation.php?api=DOMoveX"/>
		/// It won't happen for some reason when Rigidbody is not kinematic.
		/// </summary>
		private bool IsSplitEasingByAxesWorks()
		{
			return SplitEasingByAxes && IsRigidbodyNotKinematic();
		}
	}
}