using UnityEngine;
using UnityEditor;

namespace SearchPRO {
	public interface ISearchInterface {

		void OnEnable();

		void OnDisable();

		void OnGUI();

		void OnSelectionChange();
	}
}
