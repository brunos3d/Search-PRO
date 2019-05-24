#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using System.Text.RegularExpressions;

namespace SearchPRO {
	public static class GOCommands {

		[Command]
		[Category("GameObject")]
		[Title("Rename")]
		[Description("Gives a new name to object.")]
		public static void RenameGO(string search) {
			Undo.RecordObjects(Selection.gameObjects, "Rename GameObject(s)");
			string new_name = Regex.Replace(search, "rename", string.Empty, RegexOptions.IgnoreCase);
			if (new_name[0] == ' ') {
				new_name = new_name.Remove(0, 1);
			}
			int index = 0;
			foreach (GameObject go in Selection.gameObjects) {
				if (Selection.gameObjects.Length == 1) {
					go.name = new_name;
				}
				else {
					go.name = string.Format("{0} ({1})", new_name, index);
				}
				index++;
			}
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
		[Title("Duplicate")]
		[Description("Creates a clone of selected object.")]
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
