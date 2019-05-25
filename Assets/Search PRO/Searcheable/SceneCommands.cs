#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityObject = UnityEngine.Object;


namespace SearchPRO {
	public static class SceneCommands {

		[Command]
		[Category("Scene")]
		[Title("Save Scene")]
		[Description("Opens a dialog window to save the current scene.")]
		public static void SaveScene() {
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}
	}
}

#endif
