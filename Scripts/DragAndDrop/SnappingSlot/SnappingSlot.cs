using UnityEngine;

namespace Toolkit.Tweens.DragAndDrop
{
	[RequireComponent(typeof(Collider))]
	public abstract class SnappingSlot<TTarget> : MonoBehaviour, ISnappingSlot<TTarget>
	{
		public Color PreviewColor = Color.yellow;

		[SerializeField]
		protected MeshRenderer[] _meshes;

		private static readonly int ColorProperty = Shader.PropertyToID("_Color");
		private Color[] _initialColors;
		private MaterialPropertyBlock _propertyBlock;

		public Vector3 Position => transform.position;

		private void Awake()
		{
			_initialColors = new Color[_meshes.Length];
			_propertyBlock = new MaterialPropertyBlock();

			for (int i = 0; i < _meshes.Length; i++)
				_initialColors[i] = _meshes[i].sharedMaterial.color;
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
				{
					_meshes[i].GetPropertyBlock(_propertyBlock);
					_propertyBlock.SetColor(ColorProperty, _initialColors[i]);
					_meshes[i].SetPropertyBlock(_propertyBlock);
				}
			}
		}

		protected void SetMeshesColor(Color color)
		{
			foreach (MeshRenderer mesh in _meshes)
			{
				mesh.GetPropertyBlock(_propertyBlock);
				_propertyBlock.SetColor(ColorProperty, color);
				mesh.SetPropertyBlock(_propertyBlock);
			}
		}
	}
}