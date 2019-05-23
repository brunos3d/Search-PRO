#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	public static class TransformCommands {

		[Command("Reset Position", "Moves the selected object to zero position")]
		public static void ResetPosition() {
			Undo.RecordObjects(Selection.transforms, "Reset Position");
			foreach (var tr in Selection.transforms) {
				tr.position = Vector3.zero;
			}
		}

		[Command("Reset Rotation", "Sets the rotation of the selected object to zero")]
		public static void ResetRotation() {
			Undo.RecordObjects(Selection.transforms, "Reset Rotation");
			foreach (var tr in Selection.transforms) {
				tr.rotation = Quaternion.identity;
			}
		}

		[Command("Selection LookAt", "Makes all selected objects to look at the last selected object.")]
		public static void SelectionLookAt() {
			Undo.RecordObjects(Selection.transforms, "Selection LookAt");
			int last = Selection.gameObjects.Length;
			for (int id = 0; id < last; id++) {
				if (id < last - 1) {
					Selection.gameObjects[id].transform.LookAt(Selection.gameObjects[last - 1].transform);
				}
			}
		}
	}
}

#endif
