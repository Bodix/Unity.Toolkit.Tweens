// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System;
using UnityEngine.Serialization;

namespace Toolkit.Tweens.Transitions
{
    [Serializable]
    public struct CompositeTransitionPart
    {
	    [FormerlySerializedAs("TweenBehaviour")]
        public TweenTransition Transition;
        public float Position;
    }
}