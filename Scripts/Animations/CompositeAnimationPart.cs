// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System;

namespace Toolkit.Tweens.Animations
{
	[Serializable]
	public struct CompositeAnimationPart
	{
		public TweenAnimation Animation;
		public float Position;
	}
}