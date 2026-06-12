using DG.Tweening;

namespace Toolkit.Tweens.DragAndDrop
{
	public abstract class TweenedSnappable<TTarget> : Snappable<TTarget>
	{
		public float SnappingDuration = 0.3f;
		public Ease SnappingEase = Ease.OutExpo;

		public Tween SnappingTween { get; private set; }

		protected override void SnapTransition()
		{
			SnappingTween = transform.DOBlendableMoveBy(Slot.Position - transform.position, SnappingDuration)
				.SetEase(SnappingEase)
				.SetLink(gameObject)
				.OnKill(() => SnappingTween = null);
		}
	}
}