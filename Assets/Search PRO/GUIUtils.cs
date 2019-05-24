#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace SearchPRO {
	public static class GUIUtils {

		public static bool IsNullOrEmpty(this string input) {
			return (input == null || input.Length == 0);
		}

		public static bool IsNullOrWhiteSpace(this string input) {
			if (input == null) return true;

			for (int i = 0; i < input.Length; i++) {
				if (!Char.IsWhiteSpace(input[i])) return false;
			}

			return true;
		}

		public static float GetTextWidth(string text) {
			return GetTextSize(text, EditorStyles.label).x;
		}

		public static float GetTextWidth(string text, GUIStyle style) {
			return GetTextSize(text, style).x;
		}

		public static float GetTextHeight(string text) {
			return GetTextSize(text, EditorStyles.label).y;
		}

		public static float GetTextHeight(string text, GUIStyle style) {
			return GetTextSize(text, style).y;
		}

		public static Vector2 GetTextSize(string text) {
			GUIContent content = new GUIContent(text);
			return EditorStyles.label.CalcSize(content);
		}

		public static Vector2 GetTextSize(string text, GUIStyle style) {
			GUIContent content = new GUIContent(text);
			return style.CalcSize(content);
		}

		public static Vector2 GetTextSize(GUIContent content) {
			return EditorStyles.label.CalcSize(content);
		}

		public static Vector2 GetTextSize(GUIContent content, GUIStyle style) {
			return style.CalcSize(content);
		}
	}
}
#endif
