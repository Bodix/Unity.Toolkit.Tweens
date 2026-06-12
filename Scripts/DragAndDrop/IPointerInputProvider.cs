using System;
using UnityEngine;

namespace Toolkit.Tweens.DragAndDrop
{
	public interface IPointerInputProvider
	{
		event Action OnPointerDown;
		event Action OnPointerUp;

		bool PointerPressed { get; }
		Vector2 PointerPosition { get; }
		bool IsPointerOverUI(out float distanceToUI);
	}
}