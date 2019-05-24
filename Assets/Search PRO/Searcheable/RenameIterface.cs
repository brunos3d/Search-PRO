#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	[SearchInterface(false)]
	[Category("GameObject")]
	[Title("Rename...")]
	[Description("Gives a new name to GameObject(s).")]
	[Tags("GOR", "GameObject", "Selection", "Interface")]
	public class RenameIterface : ISearchInterface {

		public void OnDisable() {
		}

		public void OnEnable() {
		}

		public void OnGUI() {
			GUILayout.Button("Hello Wolrd");
		}

		public void OnSelectionChange() {
		}
	}
}

#endif
