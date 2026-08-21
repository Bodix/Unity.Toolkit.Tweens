// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System.Collections.Generic;
using System.Linq;
using Bodix.Evolunity.Extensions;
using DG.Tweening;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Screens
{
	public static class ScreenStack
	{
		private static readonly Stack<Screen> Stack = new Stack<Screen>(3);
		private static Tween transition;

		public static Screen CurrentScreen => Stack.IsEmpty() ? null : Stack.Peek();
		public static bool IsInTransition => transition != null;

		public static Tween Push(Screen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePushTween(sequence, screen));

			foreach (Screen otherScreen in Stack)
			{
				if (!otherScreen.IsEnabled)
					continue;

				if (otherScreen is AnimatedScreen otherAnimatedScreen)
					// Uncomment if you want to hide all screens in parallel.
					// sequence.Join(otherAnimatedScreen.HideTween);
					sequence.Append(otherAnimatedScreen.HideTween);
				else
					sequence.AppendCallback(otherScreen.Hide);
			}

			if (screen is AnimatedScreen animatedScreen)
				sequence.Append(animatedScreen.ShowTween
					.AddOnComplete(() => Stack.Push(screen)));
			else
				sequence.OnComplete(() =>
				{
					screen.Show();
					Stack.Push(screen);
				});

			sequence.OnKill(() => transition = null);
			return sequence;
		}

		public static Tween Pop(Screen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePopTween(sequence, screen));

			if (screen is AnimatedScreen animatedScreen)
				sequence.Append(animatedScreen.HideTween
					.AddOnComplete(() => Stack.Pop()));
			else
				sequence.AppendCallback(() =>
				{
					screen.Hide();
					Stack.Pop();
				});

			Screen nextScreen = GetNextScreen();
			if (nextScreen != null)
				if (nextScreen is AnimatedScreen nextAnimatedScreen)
					sequence.Append(nextAnimatedScreen.ShowTween);
				else
					sequence.AppendCallback(nextScreen.Show);

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
				sequence.Append(animatedScreen.ShowTween
					.AddOnStart(() =>
					{
						foreach (Screen otherScreen in Stack)
							if (otherScreen.IsEnabled)
								otherScreen.Hide();
					})
					.AddOnComplete(() => Stack.Push(screen)));

				sequence.OnKill(() => transition = null);
				return sequence;
			}
			else
			{
				if (!ValidatePush(screen))
					return DOTween.Sequence();

				foreach (Screen otherScreen in Stack)
					if (otherScreen.IsEnabled)
						otherScreen.Hide();

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
				sequence.Append(animatedScreen.HideTween
					.AddOnComplete(() =>
					{
						Stack.Pop();
						CurrentScreen?.Show();
					}));

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

		private static void ValidatePushTween(Tween tween, Screen screen)
		{
			if (transition == null || !transition.IsActive() || transition.IsComplete())
				transition = tween;

			if (transition != tween)
			{
				if (screen.LogWarnings)
					Debug.LogWarning("Failed to show the screen during the transition.");

				tween.Kill();

				return;
			}

			if (!ValidatePush(screen))
			{
				transition = null;
				tween.Kill();
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

		private static void ValidatePopTween(Tween tween, Screen screen)
		{
			if (transition == null || !transition.IsActive() || transition.IsComplete())
				transition = tween;

			if (transition != tween)
			{
				if (screen.LogWarnings)
					Debug.LogWarning("Failed to hide the screen during the transition.");

				tween.Kill();

				return;
			}

			if (!ValidatePop(screen))
			{
				transition = null;
				tween.Kill();
			}
		}

		private static Screen GetNextScreen()
		{
			return Stack.ElementAtOrDefault(1);
		}
	}
}