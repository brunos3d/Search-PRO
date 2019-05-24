#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	public static class GOCommands {

		[Command]
		[Category("GameObject")]
		[Title("Rename")]
		[Description("Gives a new name to object.")]
		public static void RenameGO(string search) {
		}

		[Command]
		[Category("GameObject")]
		[Title("Destroy Selection")]
		[Description("Will remove selected objects from your scene.")]
		public static void DestroyGameObject(GameObject[] objs) {
			foreach (GameObject go in objs) {
				if (go.activeInHierarchy) {
					Undo.DestroyObjectImmediate(go);
				}
			}
		}

		[Command]
		[Category("GameObject")]
		[Title("Destroy")]
		[Description("Will remove the object from your scene.")]
		public static void DestroyGameObject(GameObject go) {
			if (go.activeInHierarchy) {
				Undo.DestroyObjectImmediate(go);
			}
		}

		[Command]
		[Category("GameObject")]
		[Title("Duplicate")]
		[Description("Creates a clone of selected object.")]
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
