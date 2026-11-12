// Copyright © 2026 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	// Takes the element across its parent from edge to edge once: a shine over a plate, a scan line.
	// The way is counted in shares of the parent, so it fits a parent of any size. Before and after the sweep
	// the element waits outside the parent, so the parent should hide it with a mask.
	// The size of the element along the way is its own size, the other axis stays as it is.
	public class SweepAnimation : TweenAnimation<RectTransform>
	{
		public SweepDirection Direction = SweepDirection.LeftToRight;
		public float Duration = 1;
		public CustomizableEase Ease = new CustomizableEase(DG.Tweening.Ease.InOutSine);

		public override Tween Play()
		{
			InitializeIfRequired();

			SetProgress(0);

			return DOVirtual.Float(0, 1, Duration, SetProgress)
				.SetEase(Ease)
				.SetLink(Target.gameObject);
		}

		// At 0 the element stands right before the start edge of the parent, at 1 right after the end edge.
		private void SetProgress(float progress)
		{
			bool isVertical = Direction == SweepDirection.BottomToTop || Direction == SweepDirection.TopToBottom;
			bool isBackward = Direction == SweepDirection.RightToLeft || Direction == SweepDirection.TopToBottom;
			int axis = isVertical ? 1 : 0;
			float share = isBackward ? 1 - progress : progress;

			Vector2 anchorMin = Target.anchorMin;
			Vector2 anchorMax = Target.anchorMax;
			Vector2 pivot = Target.pivot;
			Vector2 position = Target.anchoredPosition;
			anchorMin[axis] = share;
			anchorMax[axis] = share;
			pivot[axis] = 1 - share;
			position[axis] = 0;

			Target.anchorMin = anchorMin;
			Target.anchorMax = anchorMax;
			Target.pivot = pivot;
			Target.anchoredPosition = position;
		}
	}

	public enum SweepDirection
	{
		LeftToRight,
		RightToLeft,
		BottomToTop,
		TopToBottom
	}
}
