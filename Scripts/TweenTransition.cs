// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using System;
using DG.Tweening;
using PerfectCore.PerfectFoundation;
using PerfectCore.PerfectFoundation.NaughtyAttributes;
using Toolkit.Tweens.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Toolkit.Tweens
{
	public abstract class TweenTransition : MonoBehaviour, IShowHideAnimations
	{
		[Tooltip("Keeps playing when the game is paused with Time.timeScale = 0. Set it on what an element plays, "
			+ "not on the parts of a composite: they follow the composite.")]
		public bool IgnoreTimeScale = false;

		public IAnimation ShowAnimation => new TransitionShowAnimation(this);
		public IAnimation HideAnimation => new TransitionHideAnimation(this);

		public abstract Tween PlayIn();

		public abstract Tween PlayOut();

		public void PlayShow(Action onStart = null, Action onComplete = null)
		{
			Tween tween = PlayIn();
			if (IgnoreTimeScale)
				tween.SetUpdate(true);
			if (onStart != null)
				tween.AddOnStart(onStart.Invoke);
			if (onComplete != null)
				tween.AddOnComplete(onComplete.Invoke);
		}

		public void PlayHide(Action onStart = null, Action onComplete = null)
		{
			Tween tween = PlayOut();
			if (IgnoreTimeScale)
				tween.SetUpdate(true);
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
		[SerializeField, FormerlySerializedAs("SameGameObjectWithTarget")]
		private TweenTargetSource _targetSource = TweenTargetSource.Assigned;
		[SerializeField, ShowIf(nameof(_targetSource), TweenTargetSource.Assigned)]
		private TComponent _target;
		// Not saved: a preset finds the element it lies on again after a move to another element.
		private TComponent _parentTarget;

		public TComponent Target
		{
			get => _targetSource == TweenTargetSource.Parent ? _parentTarget : _target;
			set
			{
				_target = value;
				_targetSource = TweenTargetSource.Assigned;
			}
		}

		protected virtual void Awake()
		{
			InitializeIfRequired();
		}

		protected virtual void OnValidate()
		{
			InitializeIfRequired();

			// A preset asset has no parent yet.
			bool isTargetExpected = _targetSource == TweenTargetSource.Self
				|| (_targetSource == TweenTargetSource.Parent && transform.parent);
			string owner = _targetSource == TweenTargetSource.Self ? "object" : "parent";
			if (isTargetExpected && !Target)
				Debug.LogWarning($"{GetType().Name} found no {typeof(TComponent).Name} on its {owner}.", this);
		}

		protected void InitializeIfRequired()
		{
			// TODO: Optimize case with Transform component. [#optimization]
			if (_targetSource == TweenTargetSource.Self && !_target)
				_target = GetComponent<TComponent>();
			else if (_targetSource == TweenTargetSource.Parent)
				_parentTarget = transform.parent ? transform.parent.GetComponent<TComponent>() : null;
		}
	}
}