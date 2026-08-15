// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using Bodix.Evolunity.Services;
using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens.Screens
{
	public abstract class AbstractScreen : MonoBehaviour, IBackHandler
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

		/// <summary>
		/// Pops the screen when back navigation is triggered.
		/// </summary>
		public virtual bool OnBackPressed()
		{
			if (!IsEnabled || ScreenStack.CurrentScreen != this)
				return false;

			Pop();

			return true;
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