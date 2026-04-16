// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using Toolkit.Tweens.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Toolkit.Tweens.Screens
{
	public class AnimatedScreen : AbstractAnimatedScreen
	{
		[SerializeField, FormerlySerializedAs("_tweenBehaviour")]
		private TweenTransition _transition;

		public override Tween ShowTween => _transition.PlayIn().AddOnStart(Show);
		public override Tween HideTween => _transition.PlayOut().AddOnComplete(Hide);
	}
}