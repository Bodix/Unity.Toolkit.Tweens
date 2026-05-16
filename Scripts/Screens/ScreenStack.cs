// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System;
using System.Collections.Generic;
using Bodix.Evolunity.Extensions;
using DG.Tweening;
using Toolkit.Tweens.Extensions;

namespace Toolkit.Tweens.Screens
{
	public static class ScreenStack
	{
		private static readonly Stack<AbstractScreen> Stack = new Stack<AbstractScreen>(3);
		private static Tween transition;

		public static AbstractScreen CurrentScreen => Stack.IsEmpty() ? null : Stack.Peek();
		public static bool IsInTransition => transition != null;

		public static Tween Push(AbstractScreen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePushTween(sequence, screen));
			foreach (AbstractScreen otherScreen in Stack)
			{
				if (otherScreen.IsEnabled)
					if (otherScreen is AbstractAnimatedScreen otherAnimatedScreen)
						sequence.Append(otherAnimatedScreen.HideTween);
					else sequence.AppendCallback(otherScreen.Hide);
			}

			sequence.OnComplete(() =>
			{
				screen.Show();
				Stack.Push(screen);
			});
			sequence.OnKill(() => transition = null);

			return sequence;
		}

		public static Tween Push(AbstractAnimatedScreen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePushTween(sequence, screen));
			foreach (AbstractScreen otherScreen in Stack)
			{
				if (otherScreen.IsEnabled)
					if (otherScreen is AbstractAnimatedScreen otherAnimatedScreen)
						sequence.Append(otherAnimatedScreen.HideTween);
					else sequence.AppendCallback(otherScreen.Hide);
			}

			sequence.Append(screen.ShowTween
				.AddOnComplete(() => Stack.Push(screen)));
			sequence.OnKill(() => transition = null);

			return sequence;
		}

		public static Tween Pop(AbstractScreen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePopTween(sequence, screen));
			sequence.AppendCallback(() =>
			{
				screen.Hide();
				Stack.Pop();
			});
			AbstractScreen nextScreen = GetNextScreen();
			if (nextScreen != null)
				if (nextScreen is AbstractAnimatedScreen nextAnimatedScreen)
					sequence.Append(nextAnimatedScreen.ShowTween);
				else sequence.AppendCallback(nextScreen.Show);
			sequence.OnKill(() => transition = null);

			return sequence;
		}

		public static Tween Pop(AbstractAnimatedScreen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePopTween(sequence, screen));
			sequence.Append(screen.HideTween
				.AddOnComplete(() => Stack.Pop()));
			AbstractScreen nextScreen = GetNextScreen();
			if (nextScreen != null)
				if (nextScreen is AbstractAnimatedScreen nextAnimatedScreen)
					sequence.Append(nextAnimatedScreen.ShowTween);
				else sequence.AppendCallback(nextScreen.Show);
			sequence.OnKill(() => transition = null);

			return sequence;
		}

		public static Tween PopCurrentScreen()
		{
			if (CurrentScreen is AbstractAnimatedScreen currentAnimatedScreen)
				return Pop(currentAnimatedScreen);
			else return Pop(CurrentScreen);
		}

		public static void PushImmediately(AbstractScreen screen)
		{
			CheckPushForExceptions(screen);

			foreach (AbstractScreen otherScreen in Stack)
				if (otherScreen.IsEnabled)
					otherScreen.Hide();

			screen.Show();
			Stack.Push(screen);
		}

		public static Tween PushImmediately(AbstractAnimatedScreen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePushTween(sequence, screen));
			sequence.Append(screen.ShowTween
				.AddOnStart(() =>
				{
					foreach (AbstractScreen otherScreen in Stack)
						if (otherScreen.IsEnabled)
							otherScreen.Hide();
				})
				.AddOnComplete(() => Stack.Push(screen)));
			sequence.OnKill(() => transition = null);

			return sequence;
		}

		public static void PopImmediately(AbstractScreen screen)
		{
			CheckPopForExceptions(screen);

			screen.Hide();
			Stack.Pop();

			CurrentScreen?.Show();
		}

		public static Tween PopImmediately(AbstractAnimatedScreen screen)
		{
			Sequence sequence = DOTween.Sequence();
			sequence.PrependCallback(() => ValidatePopTween(sequence, screen));
			sequence.Append(screen.HideTween
				.AddOnComplete(() =>
				{
					Stack.Pop();

					CurrentScreen?.Show();
				}));
			sequence.OnKill(() => transition = null);

			return sequence;
		}

		public static Tween PopImmediatelyCurrentScreen()
		{
			if (CurrentScreen is AbstractAnimatedScreen currentAnimatedScreen)
				return PopImmediately(currentAnimatedScreen);
			else
			{
				PopImmediately(CurrentScreen);
				return DOTween.Sequence();
			}
		}

		public static void Clear()
		{
			Stack.Clear();
		}

		private static void CheckPushForExceptions(AbstractScreen screen)
		{
			if (Stack.Contains(screen))
				if (CurrentScreen == screen)
					throw new InvalidOperationException("Failed to show the screen that is already shown");
				else
					throw new InvalidOperationException(
						"Failed to show the screen that is already shown under the current screen");
		}

		private static void ValidatePushTween(Tween tween, AbstractScreen screen)
		{
			try
			{
				if (transition == null || !transition.IsActive() || transition.IsComplete())
					transition = tween;
				if (transition != tween)
					throw new InvalidOperationException("Failed to show the screen during the transition");

				CheckPushForExceptions(screen);
			}
			catch (InvalidOperationException)
			{
				tween.Kill();

				throw;
			}
		}

		private static void CheckPopForExceptions(AbstractScreen screen)
		{
			if (screen != CurrentScreen)
				if (Stack.Contains(screen))
					throw new InvalidOperationException(
						"Failed to hide the screen that is under the current screen");
				else
					throw new InvalidOperationException(
						"Failed to hide the screen that is not managed by screen stack");
		}

		private static void ValidatePopTween(Tween tween, AbstractScreen screen)
		{
			try
			{
				if (transition == null || !transition.IsActive() || transition.IsComplete())
					transition = tween;
				if (transition != tween)
					throw new InvalidOperationException("Failed to hide the screen during the transition");

				CheckPopForExceptions(screen);
			}
			catch (InvalidOperationException)
			{
				tween.Kill();

				throw;
			}
		}

		private static AbstractScreen GetNextScreen()
		{
			AbstractScreen screen = CurrentScreen;
			if (CurrentScreen == null)
				return null;

			Stack.Pop();
			AbstractScreen nextScreen = CurrentScreen;
			Stack.Push(screen);

			return nextScreen;
		}
	}
}