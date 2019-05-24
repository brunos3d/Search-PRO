#if UNITY_EDITOR
using UnityEngine;

namespace SearchPRO {
	public class SearchItem {

		public object data;

		public SearchItem() { }

		public SearchItem(object data) {
			this.data = data;
		}
	}
}
#endif

