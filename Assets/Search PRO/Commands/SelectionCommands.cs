#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using System.Linq;

namespace SearchPRO {
	public static class SelectionCommands {

		[Command("Select All",
			"Select all objects in scene",
			ValidationMode.EditorCommand)]
		public static void SelectAll() {
			Selection.objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => go.hideFlags == HideFlags.None).ToArray();
		}

		[Command("Clear Selection",
			"Clear selected objects",
			ValidationMode.EditorCommand)]
		public static void ClearSelection() {
			Selection.objects = null;
		}
	}
}

#endif
