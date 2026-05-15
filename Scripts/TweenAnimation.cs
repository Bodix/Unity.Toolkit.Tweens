// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System;
using DG.Tweening;
using Evolutex.Evolunity.Components.Animations;
using NaughtyAttributes;
using Toolkit.Tweens.Extensions;
using UnityEngine;

namespace Toolkit.Tweens
{
	public abstract class TweenAnimation : MonoBehaviour, IAnimation
	{
		public abstract Tween Play();

		public void Play(Action onStart = null, Action onComplete = null)
		{
			Tween tween = Play();
			if (onStart != null)
				tween.AddOnStart(onStart.Invoke);
			if (onComplete != null)
				tween.AddOnComplete(onComplete.Invoke);
		}

		[Button("Play")]
		protected void TestPlay()
		{
			Play();
		}
	}

	public abstract class TweenAnimation<TComponent> : TweenAnimation where TComponent : Component
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