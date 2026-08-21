// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Bodix.Evolunity.Extensions;
using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens.Screens
{
	public static class ScreenStack
	{
		private static readonly Stack<Screen> Stack = new Stack<Screen>(3);
		private static Tween transition;

		public static Screen CurrentScreen => Stack.IsEmpty() ? null : Stack.Peek();
		public static bool IsInTransition => transition != null && transition.IsActive() && !transition.IsComplete();

		public static Tween Push(Screen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePushTween(sequence, screen));
			foreach (Screen otherScreen in Stack)
				if (otherScreen.IsEnabled)
					sequence.AppendHide(otherScreen);
			sequence.AppendShow(screen);
			sequence.AppendCallback(() => Stack.Push(screen));
			sequence.OnKill(() => transition = null);

			return sequence;
		}

		public static Tween Pop(Screen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePopTween(sequence, screen));
			sequence.AppendHide(screen);
			sequence.AppendCallback(() => Stack.Pop());
			Screen nextScreen = GetNextScreen();
			if (nextScreen != null)
				sequence.AppendShow(nextScreen);
			sequence.OnKill(() => transition = null);

			return sequence;
		}

		public static Tween PopCurrentScreen()
		{
			return Pop(CurrentScreen);
		}

		public static Tween PushImmediately(Screen screen)
		{
			if (screen is AnimatedScreen animatedScreen)
			{
				Sequence sequence = DOTween.Sequence();
				sequence.PrependCallback(() => ValidatePushTween(sequence, screen));
				sequence.AppendCallback(HideAllActiveScreensInstantly);
				sequence.AppendShow(animatedScreen);
				sequence.AppendCallback(() => Stack.Push(screen));
				sequence.OnKill(() => transition = null);

				return sequence;
			}
			else
			{
				if (!ValidatePush(screen))
					return DOTween.Sequence();

				HideAllActiveScreensInstantly();
				screen.Show();
				Stack.Push(screen);
				return DOTween.Sequence();
			}
		}

		public static Tween PopImmediately(Screen screen)
		{
			if (screen is AnimatedScreen animatedScreen)
			{
				Sequence sequence = DOTween.Sequence();
				sequence.PrependCallback(() => ValidatePopTween(sequence, screen));
				sequence.AppendHide(animatedScreen);
				sequence.AppendCallback(() =>
				{
					Stack.Pop();
					CurrentScreen?.Show();
				});
				sequence.OnKill(() => transition = null);

				return sequence;
			}
			else
			{
				if (!ValidatePop(screen))
					return DOTween.Sequence();

				screen.Hide();
				Stack.Pop();
				CurrentScreen?.Show();

				return DOTween.Sequence();
			}
		}

		public static Tween PopImmediatelyCurrentScreen()
		{
			return PopImmediately(CurrentScreen);
		}

		public static void Clear()
		{
			Stack.Clear();
		}

		private static void AppendShow(this Sequence sequence, Screen screen)
		{
			if (screen is AnimatedScreen animatedScreen)
				sequence.Append(animatedScreen.ShowTween);
			else
				sequence.AppendCallback(screen.Show);
		}

		private static void AppendHide(this Sequence sequence, Screen screen)
		{
			if (screen is AnimatedScreen animatedScreen)
			{
				// Uncomment if you want to hide all screens in parallel.
				// sequence.Join(animatedScreen.HideTween);
				sequence.Append(animatedScreen.HideTween);
			}
			else
			{
				sequence.AppendCallback(screen.Hide);
			}
		}

		private static void HideAllActiveScreensInstantly()
		{
			foreach (Screen otherScreen in Stack)
				if (otherScreen.IsEnabled)
					otherScreen.Hide();
		}

		private static bool ValidateAndSetTweenTransition(Screen screen, Tween tween)
		{
			if (!IsInTransition)
				transition = tween;

			if (transition != tween)
			{
				if (screen.LogWarnings)
					Debug.LogWarning("Failed to execute transition. Another transition is already in progress.");

				tween.Kill();

				return false;
			}

			return true;
		}

		private static void ValidatePushTween(Tween tween, Screen screen)
		{
			if (ValidateAndSetTweenTransition(screen, tween))
				if (!ValidatePush(screen))
				{
					transition.Kill();
					transition = null;
				}
		}

		private static bool ValidatePush(Screen screen)
		{
			if (Stack.Contains(screen))
			{
				if (screen.LogWarnings)
					Debug.LogWarning(CurrentScreen == screen
						? "Failed to show the screen that is already shown."
						: "Failed to show the screen that is already shown under the current screen.");

				return false;
			}

			return true;
		}

		private static void ValidatePopTween(Tween tween, Screen screen)
		{
			if (ValidateAndSetTweenTransition(screen, tween))
				if (!ValidatePop(screen))
				{
					transition.Kill();
					transition = null;
				}
		}

		private static bool ValidatePop(Screen screen)
		{
			if (screen != CurrentScreen)
			{
				if (screen.LogWarnings)
					Debug.LogWarning(Stack.Contains(screen)
						? "Failed to hide the screen that is under the current screen."
						: "Failed to hide the screen that is not managed by screen stack.");

				return false;
			}

			return true;
		}

		private static Screen GetNextScreen()
		{
			return Stack.ElementAtOrDefault(1);
		}
	}
}