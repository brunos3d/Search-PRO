#if UNITY_EDITOR
using UnityEngine;

namespace SearchPRO {
	[SearchInterface(false)]
	[Category("GameObject")]
	[Title("Rename...")]
	[Description("Gives a new name to GameObject(s).")]
	[Tags("GOR")]
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
