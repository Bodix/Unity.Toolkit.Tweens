// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;

namespace Toolkit.Tweens.Animations
{
	public class SequenceAnimation : TweenAnimation
	{
		public TweenAnimation[] Animations;

		public override Tween Play()
		{
			Sequence sequence = DOTween.Sequence();

			foreach (TweenAnimation animation in Animations)
				sequence.Append(animation.Play());

			return sequence;
		}
	}
}