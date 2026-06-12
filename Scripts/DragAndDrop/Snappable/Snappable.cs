using UnityEngine;

namespace Toolkit.Tweens.DragAndDrop
{
	public abstract class Snappable<TTarget> : MonoBehaviour, ISnappable<TTarget>
	{
		public Vector3 Direction = Vector3.down;
		public Quaternion DirectionRotation = Quaternion.identity;
		public float Distance = 3f;
		public LayerMask LayerMask;

		protected ISnappingSlot<TTarget> PreviewSlot;
		private RaycastHit[] _hitsBuffer;

		public abstract TTarget Target { get; }
		public ISnappingSlot<TTarget> Slot { get; set; }
		public bool IsEnabled { get => enabled; set => enabled = value; }

		protected virtual void Awake()
		{
			_hitsBuffer = new RaycastHit[8];
			enabled = false;
		}

		private void Update()
		{
			PreviewSnapping();
		}

		public bool TrySnapToPreviewSlot()
		{
			if (PreviewSlot != null && PreviewSlot.IsSnappableWith(Target))
			{
				Slot = PreviewSlot;
				// Start snap transition before snapping even if it's instant, because some logic at snap may want
				// to wait for transition completion. It will throw NullReferenceException if transition is not started.
				SnapTransition();
				Slot.Snap(Target);

				ResetPreviewSlot();
				return true;
			}
			else
			{
				ResetPreviewSlot();
				return false;
			}
		}

		protected virtual void SnapTransition()
		{
			transform.position = Slot.Position;
		}

		private void PreviewSnapping()
		{
			if (TryGetSnappingSlot(out ISnappingSlot<TTarget> slot))
			{
				if (slot != PreviewSlot)
				{
					PreviewSlot?.DisableSnapPreview();
					PreviewSlot = slot;
					PreviewSlot.EnableSnapPreview(Target);
				}
			}
			else
			{
				ResetPreviewSlot();
			}
		}

		private bool TryGetSnappingSlot(out ISnappingSlot<TTarget> slot)
		{
			slot = null;
			int hitsCount = Physics.RaycastNonAlloc(new Ray(transform.position, DirectionRotation * Direction),
				_hitsBuffer, Distance, LayerMask);

			if (hitsCount > 0)
			{
				float minDistance = float.MaxValue;

				for (int i = 0; i < hitsCount; i++)
				{
					RaycastHit hit = _hitsBuffer[i];
					var potentialSlot = hit.collider.GetComponent<ISnappingSlot<TTarget>>();

					if (potentialSlot != null && hit.distance < minDistance)
					{
						minDistance = hit.distance;
						slot = potentialSlot;
					}
				}
			}

			return slot != null;
		}

		private void ResetPreviewSlot()
		{
			if (PreviewSlot != null)
			{
				PreviewSlot.DisableSnapPreview();
				PreviewSlot = null;
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawRay(transform.position, DirectionRotation * Direction * Distance);
		}
	}
}