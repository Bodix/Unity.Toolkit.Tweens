// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens.Screens
{
	public class AnimatedScreen : Screen
	{
		[SerializeField]
		private TweenTransition _transition;

		public Tween ShowTween => _transition.PlayIn().AddOnStart(Show);
		public Tween HideTween => _transition.PlayOut().AddOnComplete(Hide);
	}
}