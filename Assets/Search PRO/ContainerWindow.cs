#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SearchPRO {
	public class ContainerWindow : EditorWindow {

		private bool close_on_lost_focus;

		private SearchInterface search_interface;

		public static ContainerWindow Instantiate(SearchInterface search_interface, GUIContent title, bool close_on_lost_focus) {
			ContainerWindow container = EditorWindow.CreateInstance<ContainerWindow>();
			container.search_interface = search_interface;
			container.titleContent = title;
			container.close_on_lost_focus = close_on_lost_focus;
			container.Show();
			return container;
		}

		void OnEnable() {
			if (search_interface == null) {
				this.Close();
			}
			search_interface.OnEnable();
		}

		void OnDisable() {
			if (search_interface != null) {
				search_interface.OnDisable();
			}
		}

		void OnGUI() {
			if (close_on_lost_focus && (focusedWindow != this || EditorApplication.isCompiling)) {
				this.Close();
			}
			search_interface.OnGUI();
		}

		void OnSelectionChange() {
			search_interface.OnSelectionChange();
		}
	}
}
#endif
