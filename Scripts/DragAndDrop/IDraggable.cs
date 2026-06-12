using UnityEngine;

namespace Toolkit.Tweens.DragAndDrop
{
	public interface IDraggable
	{
		Vector3 WorldPosition { get; }
		bool IsEnabled { get; set; }

		void BeginDrag();
		void Drag(Vector3 worldPosition);
		void EndDrag();
	}
}