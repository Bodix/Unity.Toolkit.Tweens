// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using Bodix.Evolunity.Structs;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Toolkit.Tweens.Transitions
{
	public class RectTransformTransition : TweenTransition<RectTransform>
	{
		public RectTransformData TargetTransform;
		public RectTransformData InitialTransform;
		public float InDuration = 1;
		public float OutDuration = 1;
		public Ease InEase = Ease.Linear;
		public Ease OutEase = Ease.Linear;

		public override Tween PlayIn()
		{
			InitializeIfRequired();

			return DOTween.Sequence()
				.Insert(0, Target.DOAnchorMin(TargetTransform.AnchorMin, InDuration))
				.Insert(0, Target.DOAnchorMax(TargetTransform.AnchorMax, InDuration))
				.Insert(0, Target.DOPivot(TargetTransform.Pivot, InDuration))
				.Insert(0, Target.DOSizeDelta(TargetTransform.SizeDelta, InDuration))
				.Insert(0, Target.DOAnchorPos3D(TargetTransform.AnchoredPosition, InDuration))
				.SetEase(InEase);
		}

		public override Tween PlayOut()
		{
			InitializeIfRequired();

			return DOTween.Sequence()
				.Insert(0, Target.DOAnchorMin(InitialTransform.AnchorMin, OutDuration))
				.Insert(0, Target.DOAnchorMax(InitialTransform.AnchorMax, OutDuration))
				.Insert(0, Target.DOPivot(InitialTransform.Pivot, OutDuration))
				.Insert(0, Target.DOSizeDelta(InitialTransform.SizeDelta, OutDuration))
				.Insert(0, Target.DOAnchorPos3D(InitialTransform.AnchoredPosition, OutDuration))
				.SetEase(OutEase);
		}

#if UNITY_EDITOR
		[Button("Copy current RectTransform to " + nameof(TargetTransform))]
		private void CopyTransformToTarget()
		{
			Undo.RecordObject(Target, "Copy current RectTransform to " + nameof(TargetTransform));
			TargetTransform = Target.GetData();
		}

		[Button("Copy current RectTransform to " + nameof(InitialTransform))]
		private void CopyTransformToInitial()
		{
			Undo.RecordObject(Target, "Copy current RectTransform to " + nameof(InitialTransform));
			InitialTransform = Target.GetData();
		}

		[Button("Set current RectTransform from " + nameof(TargetTransform))]
		private void SetTransformFromTarget()
		{
			Undo.RecordObject(Target, "Set current RectTransform from " + nameof(TargetTransform));
			Target.SetData(TargetTransform);
		}

		[Button("Set current RectTransform from " + nameof(InitialTransform))]
		private void SetTransformFromInitial()
		{
			Undo.RecordObject(Target, "Set current RectTransform from " + nameof(InitialTransform));
			Target.SetData(InitialTransform);
		}
#endif
	}
}