// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Toolkit.Tweens
{
	public abstract class TweenAnimation<TComponent> : TweenBehaviour<TComponent> where TComponent : Component
	{
		public abstract Tween Play();

		[Button("Play")]
		protected void TestPlay()
		{
			Play();
		}
	}
}