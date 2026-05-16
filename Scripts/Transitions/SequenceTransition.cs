// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using Bodix.Evolunity.Extensions;
using DG.Tweening;
using UnityEngine.Serialization;

namespace Toolkit.Tweens.Transitions
{
	public class SequenceTransition : TweenTransition
	{
		[FormerlySerializedAs("_tweenBehaviours")]
		public TweenTransition[] Transitions;

		public override Tween PlayIn()
		{
			Sequence sequence = DOTween.Sequence();

			foreach (TweenTransition transition in Transitions)
				sequence.Append(transition.PlayIn());

			return sequence;
		}

		public override Tween PlayOut()
		{
			Sequence sequence = DOTween.Sequence();

			// In reversed order.
			foreach (TweenTransition transition in Transitions.Reverse())
				sequence.Append(transition.PlayOut());

			return sequence;
		}
	}
}