// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using Bodix.Evolunity.Services;
using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens.Screens
{
	public abstract class AbstractScreen : MonoBehaviour, IBackNavigationHandler
	{
		[SerializeField]
		[Tooltip("Determines whether to log warnings when invalid operations are performed on this screen.")]
		private bool _logWarnings = true;

		public bool LogWarnings => _logWarnings;

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

			if (ScreenStack.IsInTransition)
			{
				// We return true to “catch” the user's click and prevent them from 
				// closing something else or exiting the game while the animation is playing.
				// But we don't call Pop() itself.
				return true;
			}

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

		public virtual Tween PushImmediately()
		{
			return ScreenStack.PushImmediately(this);
		}

		public virtual Tween PopImmediately()
		{
			return ScreenStack.PopImmediately(this);
		}
	}
}