// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens.Screens
{
	public abstract class AbstractScreen : MonoBehaviour
	{
		public virtual bool IsEnabled
		{
			get => gameObject.activeSelf;
			set => gameObject.SetActive(value);
		}

		public virtual void Show()
		{
			IsEnabled = true;
		}

		public virtual void Hide()
		{
			IsEnabled = false;
		}

		public virtual Tween Push()
		{
			return ScreenStack.Push(this);
		}

		public virtual Tween Pop()
		{
			return ScreenStack.Pop(this);
		}

		public void PushImmediately()
		{
			ScreenStack.PushImmediately(this);
		}

		public void PopImmediately()
		{
			ScreenStack.PopImmediately(this);
		}
	}
}