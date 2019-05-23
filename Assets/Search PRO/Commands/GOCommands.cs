#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	public static class GOCommands {

		[Command("Destroy Selection", "Will remove selected objects from your scene.")]
		public static void DestroyGameObject(GameObject[] objs) {
			foreach (GameObject go in objs) {
				if (go.activeInHierarchy) {
					Undo.DestroyObjectImmediate(go);
				}
			}
		}

		[Command("Destroy", "Will remove the object from your scene.")]
		public static void DestroyGameObject(GameObject go) {
			if (go.activeInHierarchy) {
				Undo.DestroyObjectImmediate(go);
			}
		}

		[Command("Duplicate", "Creates a clone of selected object.")]
		public static void DuplicateGameObject(GameObject[] objs) {
			foreach (GameObject go in objs) {
				if (go.activeInHierarchy) {
					UnityObject instance = UnityObject.Instantiate(go, go.transform.position, go.transform.rotation);
					Undo.RegisterCreatedObjectUndo(instance, "Duplicate");
				}
			}
		}
	}
}

#endif
