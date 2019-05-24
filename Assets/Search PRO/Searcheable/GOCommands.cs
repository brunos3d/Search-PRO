#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using System.Text.RegularExpressions;

namespace SearchPRO {
	public static class GOCommands {

		[Command]
		[Category("GameObject")]
		[Title("Destroy Selection")]
		[Description("Will remove selected objects from your scene.")]
		[Tags("GOD", "GameObject", "Selection")]
		public static void DestroyGameObject(GameObject[] objs) {
			foreach (GameObject go in objs) {
				if (go.activeInHierarchy) {
					Undo.DestroyObjectImmediate(go);
				}
			}
		}

		[Command]
		[Category("GameObject")]
		[Title("Duplicate")]
		[Description("Creates a clone of selected object.")]
		[Tags("GOC", "GameObject", "Selection")]
		public static void DuplicateGameObject(GameObject[] objs) {
			foreach (GameObject go in objs) {
				if (go.activeInHierarchy) {
					UnityObject instance = UnityObject.Instantiate(go, go.transform.position, go.transform.rotation);
					Undo.RegisterCreatedObjectUndo(instance, "Duplicate GameObject(s)");
				}
			}
		}
	}
}

#endif
