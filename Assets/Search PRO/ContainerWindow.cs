#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SearchPRO {
	public class ContainerWindow : EditorWindow {

		private class Styles {

			public GUIStyle title = (GUIStyle)"AM MixerHeader2";

			public GUIStyle description = (GUIStyle)"AM HeaderStyle";

			public Styles() { }
		}

		private Styles styles;

		private Vector2 scroll;

		private bool close_on_lost_focus;

		private ISearchInterface search_interface;

		public static ContainerWindow CreateNew(ISearchInterface search_interface, GUIContent title, bool close_on_lost_focus) {
			ContainerWindow container = EditorWindow.CreateInstance<ContainerWindow>();
			container.search_interface = search_interface;
			container.titleContent = title;
			container.close_on_lost_focus = close_on_lost_focus;
			container.Show();
			return container;
		}

		void OnEnable() {
			styles = new Styles();
			if (search_interface != null) {
				search_interface.OnEnable();
			}
		}

		void OnDisable() {
			if (search_interface != null) {
				search_interface.OnDisable();
			}
		}

		void OnGUI() {
			if (search_interface == null || (close_on_lost_focus && (focusedWindow != this || EditorApplication.isCompiling))) {
				this.Close();
			}
			GUILayout.Label(titleContent.text, styles.title);
			GUILayout.Label(titleContent.tooltip, styles.description);
			GUILayout.Space(10.0f);

			scroll = EditorGUILayout.BeginScrollView(scroll);
			search_interface.OnGUI();
			EditorGUILayout.EndScrollView();
		}

		void OnSelectionChange() {
			search_interface.OnSelectionChange();
		}
	}
}
#endif
