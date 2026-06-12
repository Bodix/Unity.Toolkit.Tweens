using UnityEngine;

namespace Toolkit.Tweens.DragAndDrop
{
	[RequireComponent(typeof(Collider))]
	public class Draggable : MonoBehaviour, IDraggable
	{
		public Vector3 WorldPosition => transform.position;
		public bool IsEnabled { get => enabled; set => enabled = value; }
		public bool IsDragging { get; private set; }

		public virtual void BeginDrag()
		{
			IsDragging = true;
		}

		public virtual void Drag(Vector3 worldPosition)
		{
			transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
		}

		public virtual void EndDrag()
		{
			IsDragging = false;
		}
	}
}