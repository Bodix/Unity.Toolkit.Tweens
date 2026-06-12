using UnityEngine;

namespace Toolkit.Tweens.DragAndDrop
{
	public class DragAndDrop : MonoBehaviour
	{
		[SerializeField]
		private LayerMask _layerMask;
		[SerializeField]
		private bool _drawDebug = true;

		private Camera _camera;
		private IPointerInputProvider _inputProvider;
		private RaycastHit[] _hitsBuffer;
		private IDraggable _currentDraggable;

		public void Construct(Camera mainCamera, IPointerInputProvider inputProvider)
		{
			_camera = mainCamera;
			_inputProvider = inputProvider;
			_hitsBuffer = new RaycastHit[128];
		}

		private void OnEnable()
		{
			if (_inputProvider != null)
			{
				_inputProvider.OnPointerDown += OnInputDown;
				_inputProvider.OnPointerUp += OnInputUp;
			}
		}

		private void OnDisable()
		{
			if (_inputProvider != null)
			{
				_inputProvider.OnPointerDown -= OnInputDown;
				_inputProvider.OnPointerUp -= OnInputUp;
			}
		}

		private void Update()
		{
			if (_inputProvider == null || _camera == null)
				return;

			OnInputHold();
		}

		private void OnInputDown()
		{
			Vector2 screenPosition = _inputProvider.PointerPosition;

			if (TryGetDraggable(screenPosition, out IDraggable draggable, out float distanceToDraggable) && draggable.IsEnabled
			    && !(_inputProvider.IsPointerOverUI(out float distanceToUI) && distanceToDraggable > distanceToUI))
			{
				_currentDraggable = draggable;
				_currentDraggable.BeginDrag();
			}
		}

		private void OnInputHold()
		{
			if (!_inputProvider.PointerPressed)
				return;

			Vector2 screenPosition = _inputProvider.PointerPosition;

			if (IsDraggableExists(_currentDraggable) && _currentDraggable.IsEnabled)
			{
				Vector3 worldPosition = ConvertScreenToWorldPoint(screenPosition, GetDistanceToCurrentDraggable());

				if (_drawDebug)
				{
					Debug.DrawLine(worldPosition - Vector3.left * 0.25f, worldPosition + Vector3.left * 0.25f, Color.green);
					Debug.DrawLine(worldPosition - Vector3.forward * 0.25f, worldPosition + Vector3.forward * 0.25f, Color.green);
				}

				_currentDraggable.Drag(worldPosition);
			}
		}

		private void OnInputUp()
		{
			if (IsDraggableExists(_currentDraggable))
			{
				_currentDraggable.EndDrag();
				_currentDraggable = null;
			}
		}

		private float GetDistanceToCurrentDraggable()
		{
			return (_currentDraggable.WorldPosition - _camera.transform.position).magnitude;
		}

		private Vector3 ConvertScreenToWorldPoint(Vector2 screenPoint, float zPos = 1f)
		{
			return _camera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, zPos));
		}

		private bool TryGetDraggable(Vector2 screenPoint, out IDraggable draggable, out float distanceToHit)
		{
			draggable = null;
			distanceToHit = 0f;

			Ray ray = _camera.ScreenPointToRay(screenPoint);
			int hitCount;

			if ((hitCount = Physics.RaycastNonAlloc(ray, _hitsBuffer, float.PositiveInfinity, _layerMask)) > 0)
			{
				float minDistance = float.MaxValue;

				for (int i = 0; i < hitCount; i++)
				{
					RaycastHit hit = _hitsBuffer[i];
					var currentDraggable = hit.collider.GetComponent<IDraggable>();

					if (currentDraggable != null && hit.distance < minDistance)
					{
						minDistance = hit.distance;
						draggable = currentDraggable;
						distanceToHit = hit.distance;
					}
				}
			}

			if (_drawDebug)
				Debug.DrawRay(ray.origin, ray.direction * 1000f, draggable != null ? Color.green : Color.yellow, 1);

			return draggable != null;
		}

		private bool IsDraggableExists(IDraggable draggable)
		{
			// Draggable may be not null but already destroyed.
			return draggable is Object unityObj && unityObj;
		}
	}
}