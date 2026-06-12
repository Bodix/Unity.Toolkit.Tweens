namespace Toolkit.Tweens.DragAndDrop
{
	public interface ISnappable<in TTarget>
	{
		bool IsEnabled { get; set; }
		ISnappingSlot<TTarget> Slot { get; }
		bool TrySnapToPreviewSlot();
	}
}