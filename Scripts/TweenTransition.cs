// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace Toolkit.Tweens
{
	public abstract class TweenTransition : MonoBehaviour
	{
		public abstract Tween PlayIn();

		public abstract Tween PlayOut();

		[Button("Play In")]
		protected void TestPlayIn()
		{
			PlayIn();
		}

		[Button("Play Out")]
		protected void TestPlayOut()
		{
			PlayOut();
		}
	}

	public abstract class TweenTransition<TComponent> : TweenTransition where TComponent : Component
	{
		[SerializeField]
		private bool SameGameObjectWithTarget = false;
		[SerializeField, HideIf(nameof(SameGameObjectWithTarget))]
		private TComponent _target;

		public TComponent Target
		{
			get => _target;
			set
			{
				_target = value;

				if (_target != transform)
					SameGameObjectWithTarget = false;
			}
		}

		protected virtual void Awake()
		{
			InitializeIfRequired();
		}

		protected virtual void OnValidate()
		{
			InitializeIfRequired();
		}

		protected void InitializeIfRequired()
		{
			// TODO: Optimize case with Transform component. [#optimization]
			if (!Target && SameGameObjectWithTarget)
				_target = GetComponent<TComponent>();
		}
	}
}