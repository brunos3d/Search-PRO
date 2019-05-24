#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SearchPRO {
	public interface SearchInterface {

		void OnEnable();

		void OnDisable();

		void OnGUI();

		void OnSelectionChange();
	}
}
#endif
