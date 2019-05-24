#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace SearchPRO {
	public class Interface {

		public void OnEnable() {

		}

		public void OnDisable() {

		}

		public void OnGUI() {
		}
	}

	public class ContainerWindow : EditorWindow {

		private Interface user_interface;

		public static ContainerWindow Instantiate(Interface user_interface, GUIContent title) {
			ContainerWindow container = EditorWindow.CreateInstance<ContainerWindow>();
			container.user_interface = user_interface;
			container.titleContent = title;
			container.Show();
			return container;
		}

		void OnEnable() {
			if (user_interface == null) {
				this.Close();
			}

			user_interface.OnEnable();
		}

		void OnDisable() {
			if (user_interface != null) {
				user_interface.OnDisable();
			}
		}

		void OnGUI() {
			user_interface.OnGUI();
		}
	}
}
#endif
