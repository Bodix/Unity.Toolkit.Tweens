// Copyright © 2026 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

namespace Toolkit.Tweens
{
	// Where a tween takes the component it animates.
	public enum TweenTargetSource
	{
		// The Target field.
		Assigned,
		// The object of the tween.
		Self,
		// The parent object: a preset animates the element it lies on.
		Parent
	}
}
