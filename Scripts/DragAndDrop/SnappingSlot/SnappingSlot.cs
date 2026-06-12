using UnityEngine;

namespace Toolkit.Tweens.DragAndDrop
{
	[RequireComponent(typeof(Collider))]
	public abstract class SnappingSlot<TTarget> : MonoBehaviour, ISnappingSlot<TTarget>
	{
		public Color PreviewColor = Color.yellow;

		[SerializeField]
		protected MeshRenderer[] _meshes;

		private Color[] _initialColors;

		public Vector3 Position => transform.position;

		private void Awake()
		{
			_initialColors = new Color[_meshes.Length];
			for (int i = 0; i < _meshes.Length; i++)
				_initialColors[i] = _meshes[i].material.color;
		}

		public abstract bool IsSnappableWith(TTarget other);

		public abstract void Snap(TTarget other);

		public virtual void EnableSnapPreview(TTarget other)
		{
			SetMeshesColor(PreviewColor);
		}

		public virtual void DisableSnapPreview()
		{
			for (int i = 0; i < _meshes.Length; i++)
			{
				if (i < _initialColors.Length)
					_meshes[i].material.color = _initialColors[i];
			}
		}

		protected void SetMeshesColor(Color color)
		{
			foreach (MeshRenderer mesh in _meshes)
				mesh.material.color = color;
		}
	}
}