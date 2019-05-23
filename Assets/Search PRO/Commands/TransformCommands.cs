#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	public static class TransformCommands {

		[Command("Reset Position",
			"Moves selected object to zero position",
			ValidationMode.AtLeastOneGameObject)]
		public static void ResetPosition() {
			foreach (var tr in Selection.transforms) {
				tr.position = Vector3.zero;
			}
		}

		[Command("Selection LookAt",
			"Makes all selected objects to look at the last selected object.",
			ValidationMode.AtLeastTwoGameObjects)]
		public static void SelectionLookAt() {
			int last_go = Selection.transforms.Length;
			for (int id = 0; id < last_go; id++) {
				if (id < last_go - 1) {
					Selection.transforms[id].LookAt(Selection.transforms[last_go - 1]);
				}
			}
		}

		[Command("Lock Selection LookAt",
			"(Lock) Makes all selected objects to look at the last selected object.",
			ValidationMode.AtLeastTwoGameObjects)]
		public static void LockSelectionLookAt() {
			GameObject[] objs = Selection.gameObjects;
			EditorApplication.update += () => {
				int last_go = objs.Length;
				for (int id = 0; id < last_go; id++) {
					if (id < last_go - 1) {
						objs[id].transform.LookAt(objs[last_go - 1].transform);
					}
				}
			};
		}
	}
}

#endif
