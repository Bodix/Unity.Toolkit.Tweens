// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System;
using DG.Tweening;
using Evolutex.Evolunity.Components.Animations;
using Evolutex.Evolunity.Components.UI;
using NaughtyAttributes;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens
{
	public abstract class TweenTransition : MonoBehaviour, IShowHideAnimations
	{
		public IAnimation ShowAnimation => new TransitionShowAnimation(this);
		public IAnimation HideAnimation => new TransitionHideAnimation(this);

		public abstract Tween PlayIn();

		public abstract Tween PlayOut();

		public void PlayShow(Action onStart = null, Action onComplete = null)
		{
			Tween tween = PlayIn();
			if (onStart != null)
				tween.AddOnStart(onStart.Invoke);
			if (onComplete != null)
				tween.AddOnComplete(onComplete.Invoke);
		}

		public void PlayHide(Action onStart = null, Action onComplete = null)
		{
			Tween tween = PlayOut();
			if (onStart != null)
				tween.AddOnStart(onStart.Invoke);
			if (onComplete != null)
				tween.AddOnComplete(onComplete.Invoke);
		}

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