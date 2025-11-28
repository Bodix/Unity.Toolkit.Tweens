// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Toolkit.Tweens
{
	public abstract class AnimationTweenBehaviour : MonoBehaviour
	{
		public abstract Tween Play();

		[Button("Play")]
		protected void TestPlay()
		{
			Play();
		}
	}
}