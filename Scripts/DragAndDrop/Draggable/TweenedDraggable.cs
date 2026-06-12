using System;
using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens.DragAndDrop
{
	[RequireComponent(typeof(TweenedSnappable<>))]
	public class TweenedDraggable<TTweenedSnappable, TTarget> : Draggable where TTweenedSnappable : TweenedSnappable<TTarget>
	{
		public float TransitionHeight = 1f;
		public float TransitionDuration = 0.3f;
		public Ease TransitionEase = Ease.OutExpo;

		private TTweenedSnappable _snappable;
		// Begin and end tween variables should be separated to prevent overriding end tween by begin tween.
		private Tween _beginDragTween;
		private Tween _endDragTween;

		public event Action OnBeginDrag;
		public event Action OnEndDrag;
		public event Action<bool> OnSnap;

		private void Awake()
		{
			_snappable = GetComponent<TTweenedSnappable>();
		}

		public override void BeginDrag()
		{
			base.BeginDrag();

			_snappable.IsEnabled = true;

			_beginDragTween = transform.DOBlendableMoveBy(new Vector3(0, TransitionHeight, 0), TransitionDuration)
				.SetEase(TransitionEase)
				.SetLink(gameObject)
				.OnKill(() => _beginDragTween = null);

			OnBeginDrag?.Invoke();
		}

		public override void Drag(Vector3 worldPosition)
		{
			transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
		}

		public override void EndDrag()
		{
			base.EndDrag();

			_snappable.IsEnabled = false;

			// Stop existing animations that have been started to prevent
			// the accumulation of blendable movements when the user clicks frequently.
			// The end drag animation should be the only one at the time, as all other additional animations
			// will shift the block from its correct final destination (because they are blendable).
			_snappable.SnappingTween.Kill();
			_beginDragTween.Kill();
			_endDragTween.Kill();

			if (_snappable.TrySnapToPreviewSlot())
			{
				OnSnap?.Invoke(true);
			}
			else
			{
				_endDragTween = transform.DOBlendableMoveBy(_snappable.Slot.Position - transform.position, TransitionDuration)
					.SetEase(TransitionEase)
					.SetLink(gameObject)
					.OnKill(() => _endDragTween = null);
				OnSnap?.Invoke(false);
			}

			OnEndDrag?.Invoke();
		}
	}
}