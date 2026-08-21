// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;

namespace Toolkit.Tweens.Screens
{
	public abstract class AbstractAnimatedScreen : AbstractScreen
	{
		public abstract Tween ShowTween { get; }

		public abstract Tween HideTween { get; }
	}
}