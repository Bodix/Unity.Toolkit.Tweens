// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using UnityEngine;

namespace Toolkit.Tweens
{
	// public abstract class TweenTransition<TComponent> : TweenBehaviour<TComponent> where TComponent : Component
	public abstract class TweenTransition : MonoBehaviour
	{
		public abstract Tween PlayIn();

		public abstract Tween PlayOut();
	}
}