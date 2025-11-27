// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System;
using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens
{
	[Serializable]
	public struct CustomizableEase
	{
		public bool IsCustom;
		public Ease Ease;
		public AnimationCurve CustomCurve;

		public CustomizableEase(Ease ease)
		{
			IsCustom = false;
			Ease = ease;
			CustomCurve = AnimationCurve.Constant(0, 1, 1);
		}

		public CustomizableEase(AnimationCurve curve)
		{
			IsCustom = true;
			Ease = Ease.Unset;
			CustomCurve = curve;
		}
	}
}