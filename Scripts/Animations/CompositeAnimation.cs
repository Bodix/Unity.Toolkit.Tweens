using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens.Animations
{
	public class CompositeAnimation : TweenAnimation
	{
		[SerializeField]
		private CompositeAnimationPart[] Animations;

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