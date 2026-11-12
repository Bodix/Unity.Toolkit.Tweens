// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System.Linq;
using DG.Tweening;
using Toolkit.Tweens.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Toolkit.Tweens.Transitions
{
	// Plays its parts at their positions on one timeline. In plays the transitions of the show In,
	// Out plays the transitions of the hide Out. Without parts of the hide, the hide is the show reversed.
	public class CompositeTransition : TweenTransition
	{
		[Tooltip("The show: every transition plays In at its position.")]
		[FormerlySerializedAs("Transitions"), FormerlySerializedAs("_tweenBehaviours")]
		public CompositeTransitionPart[] InTransitions = new CompositeTransitionPart[0];
		[Tooltip("One-off animations of the show, for example a shine. The reversed show skips them.")]
		public CompositeAnimationPart[] InAnimations = new CompositeAnimationPart[0];
		[Tooltip("The hide: every transition plays Out at its position. Without parts of the hide, the hide is the show reversed.")]
		public CompositeTransitionPart[] OutTransitions = new CompositeTransitionPart[0];
		[Tooltip("One-off animations of the hide.")]
		public CompositeAnimationPart[] OutAnimations = new CompositeAnimationPart[0];

		private bool HasOwnHide => OutTransitions.Length > 0 || OutAnimations.Length > 0;

		public override Tween PlayIn()
		{
			Sequence sequence = DOTween.Sequence();

			foreach (CompositeTransitionPart part in InTransitions)
				Insert(sequence, part.Transition.PlayIn(), part.Position);

			foreach (CompositeAnimationPart part in InAnimations)
				Insert(sequence, part.Animation.Play(), part.Position);

			return sequence;
		}

		public override Tween PlayOut()
		{
			Sequence sequence = DOTween.Sequence();

			if (HasOwnHide)
			{
				foreach (CompositeTransitionPart part in OutTransitions)
					Insert(sequence, part.Transition.PlayOut(), part.Position);

				foreach (CompositeAnimationPart part in OutAnimations)
					Insert(sequence, part.Animation.Play(), part.Position);
			}
			else
			{
				// The show in reversed order.
				float maxPosition = InTransitions.Length > 0 ? InTransitions.Max(x => x.Position) : 0;
				foreach (CompositeTransitionPart part in InTransitions)
					Insert(sequence, part.Transition.PlayOut(), maxPosition - part.Position);
			}

			return sequence;
		}

		private static void Insert(Sequence sequence, Tween part, float position)
		{
			// To prewarm tween initial state.
			// TODO: The prewarm fires OnStart of the part at once, so a part that starts a sound or an effect in OnStart
			// starts it before its position. [#bug]
			part.ManualUpdate(float.MinValue, float.MinValue);

			sequence.Insert(position, part);
		}
	}
}
