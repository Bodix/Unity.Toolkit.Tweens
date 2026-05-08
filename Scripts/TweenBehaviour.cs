using NaughtyAttributes;
using UnityEngine;

namespace Toolkit.Tweens
{
	public class TweenBehaviour<TComponent> : MonoBehaviour where TComponent : Component
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
			if (!Target && SameGameObjectWithTarget)
				_target = GetComponent<TComponent>();
		}
	}
}