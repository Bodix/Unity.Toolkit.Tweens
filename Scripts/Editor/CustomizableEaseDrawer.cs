// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Toolkit.Tweens.Editor
{
	[CustomPropertyDrawer(typeof(CustomizableEase))]
	public class CustomizableEaseDrawer : PropertyDrawer
	{
		private const float TOGGLE_WIDTH = 80f;
		private const float SPACING = 4f;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var isCustomProp = property.FindPropertyRelative(nameof(CustomizableEase.IsCustom));
			float height = EditorGUIUtility.singleLineHeight;

			if (isCustomProp.boolValue)
			{
				var curveProp = property.FindPropertyRelative(nameof(CustomizableEase.CustomCurve));
				height += EditorGUI.GetPropertyHeight(curveProp, true);
			}

			return height;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, label);

			// Save current indent, to not break indentation.
			int oldIndent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			Rect toggleRect = new Rect(position.x, position.y, TOGGLE_WIDTH, EditorGUIUtility.singleLineHeight);
			Rect easeRect = new Rect(position.x + TOGGLE_WIDTH + SPACING, position.y, position.width - TOGGLE_WIDTH - SPACING, EditorGUIUtility.singleLineHeight);

			var isCustomProp = property.FindPropertyRelative(nameof(CustomizableEase.IsCustom));
			var easeTypeProp = property.FindPropertyRelative(nameof(CustomizableEase.Ease));
			var curveProp = property.FindPropertyRelative(nameof(CustomizableEase.CustomCurve));

			bool newIsCustom = EditorGUI.ToggleLeft(toggleRect, "Custom", isCustomProp.boolValue);
			if (newIsCustom != isCustomProp.boolValue)
			{
				isCustomProp.boolValue = newIsCustom;

				if (newIsCustom)
					curveProp.animationCurveValue = AnimationCurve.EaseInOut(0, 0, 1, 1);
				else
					easeTypeProp.enumValueIndex = (int)Ease.OutQuad;
			}

			if (isCustomProp.boolValue)
			{
				// Offset down for curve.
				Rect curveRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
					position.width, EditorGUI.GetPropertyHeight(curveProp, true));

				EditorGUI.PropertyField(curveRect, curveProp, GUIContent.none);
			}
			else
			{
				EditorGUI.PropertyField(easeRect, easeTypeProp, GUIContent.none);
			}

			EditorGUI.indentLevel = oldIndent;
		}
	}
}