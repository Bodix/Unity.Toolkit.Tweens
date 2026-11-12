// Copyright © 2026 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Transitions
{
	// Comes in from an offset to its own place and leaves to an offset.
	// The own place is not saved: if a layout or a script moves the element, its new position becomes its own place,
	// so the transition works over any layout.
	public class MoveTransition : TweenTransition<Transform>
	{
		[Tooltip("Where the element comes from: an offset from its own place in the local units of its parent.")]
		public Vector3 InOffset = new Vector3(0, 100, 0);
		[Tooltip("Where the element leaves to: an offset from its own place in the local units of its parent.")]
		public Vector3 OutOffset = new Vector3(0, 100, 0);
		public float InDuration = 1;
		public float OutDuration = 1;
		public CustomizableEase InEase = new CustomizableEase(Ease.OutCubic);
		public CustomizableEase OutEase = new CustomizableEase(Ease.InCubic);

		private Vector3 _appliedOffset;
		private Vector3 _lastPosition;

		public override Tween PlayIn()
		{
			return Play(InOffset, Vector3.zero, InDuration, InEase);
		}

		public override Tween PlayOut()
		{
			return Play(Vector3.zero, OutOffset, OutDuration, OutEase);
		}

		private Tween Play(Vector3 from, Vector3 to, float duration, CustomizableEase ease)
		{
			InitializeIfRequired();

			SetOffset(from);

			// Unclamped, so an ease like Back overshoots the own place or steps back before leaving.
			return DOVirtual.Float(0, 1, duration, progress => SetOffset(Vector3.LerpUnclamped(from, to, progress)))
				.SetEase(ease)
				.SetLink(Target.gameObject);
		}

		// TODO: A layout that moves the element later in the same frame, in the rebuild of its canvas,
		// shows it at its own place for this frame. [#bug]
		private void SetOffset(Vector3 offset)
		{
			Vector3 position = Target.localPosition;
			// A position the transition did not set is the new own place.
			Vector3 ownPosition = position == _lastPosition ? position - _appliedOffset : position;

			_appliedOffset = offset;
			_lastPosition = ownPosition + offset;
			Target.localPosition = _lastPosition;
		}
	}
}
