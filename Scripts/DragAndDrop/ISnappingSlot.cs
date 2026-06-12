using UnityEngine;

namespace Toolkit.Tweens.DragAndDrop
{
	public interface ISnappingSlot<in TTarget>
	{
		Vector3 Position { get; }

		void EnableSnapPreview(TTarget other);
		void DisableSnapPreview();

		bool IsSnappableWith(TTarget other);
		void Snap(TTarget other);
	}
}