#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using System.Linq;

namespace SearchPRO {
	public static class SelectionCommands {

		[Command]
		[Category("Selection")]
		[Title("Select All")]
		[Description("Select all objects in scene.")]
		public static void SelectAll() {
			Selection.objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(go => go.hideFlags == HideFlags.None).ToArray();
		}

		[Command]
		[Category("Selection")]
		[Title("Clear Selection")]
		[Description("Clear selected objects.")]
		public static void ClearSelection() {
			Selection.objects = null;
		}
	}
}

#endif
