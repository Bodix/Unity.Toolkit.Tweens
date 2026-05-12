// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System.Linq;
using DG.Tweening;
using UnityEngine.Serialization;

namespace Toolkit.Tweens.Transitions
{
	public class CompositeTransition : TweenTransition
	{
		[FormerlySerializedAs("_tweenBehaviours")]
		public CompositeTransitionPart[] Transitions;

		public override Tween PlayIn()
		{
			Sequence sequence = DOTween.Sequence();

			foreach (CompositeTransitionPart part in Transitions)
			{
				Tween tween = part.Transition.PlayIn();

				// To prewarm tween initial state.
				tween.ManualUpdate(float.MinValue, float.MinValue);

				sequence.Insert(part.Position, tween);
			}

			return sequence;
		}

		public override Tween PlayOut()
		{
			Sequence sequence = DOTween.Sequence();

			// In reversed order.
			float maxPosition = Transitions.Select(x => x.Position).Max();
			foreach (CompositeTransitionPart part in Transitions)
			{
				Tween tween = part.Transition.PlayOut();

				// To prewarm tween initial state.
				tween.ManualUpdate(float.MinValue, float.MinValue);

				sequence.Insert(maxPosition - part.Position, tween);
			}

			return sequence;
		}
	}
}