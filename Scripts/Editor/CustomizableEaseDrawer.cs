// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using UnityEditor;
using UnityEngine;

namespace Toolkit.Tweens.Editor
{
	[CustomPropertyDrawer(typeof(CustomizableEase))]
	public class CustomizableEaseDrawer : PropertyDrawer
	{
		private const float ToggleWidth = 70f;
		private const float Spacing = 4f;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, label);

			Rect toggleRect = new Rect(position.x, position.y, ToggleWidth, EditorGUIUtility.singleLineHeight);
			Rect propertyRect = new Rect(position.x + ToggleWidth + Spacing, position.y,
				position.width - ToggleWidth - Spacing, EditorGUIUtility.singleLineHeight);
			SerializedProperty isCustomProperty = property.FindPropertyRelative(nameof(CustomizableEase.IsCustom));
			SerializedProperty easeProperty = property.FindPropertyRelative(nameof(CustomizableEase.Ease));
			SerializedProperty customCurveProperty = property.FindPropertyRelative(nameof(CustomizableEase.CustomCurve));

			isCustomProperty.boolValue = EditorGUI.ToggleLeft(toggleRect, "Custom", isCustomProperty.boolValue);
			EditorGUI.PropertyField(propertyRect, isCustomProperty.boolValue ? customCurveProperty : easeProperty, GUIContent.none);
		}
	}
}