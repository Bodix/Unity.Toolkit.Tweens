// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;

namespace Toolkit.Tweens.Animations
{
	public class CompositeAnimation : TweenAnimation
	{
		public CompositeAnimationPart[] Animations;

		public override Tween Play()
		{
			Sequence sequence = DOTween.Sequence();

			foreach (CompositeAnimationPart part in Animations)
			{
				Tween tween = part.Animation.Play();

				// To prewarm tween initial state.
				tween.ManualUpdate(float.MinValue, float.MinValue);

				sequence.Insert(part.Position, tween);
			}

			return sequence;
		}
	}
}