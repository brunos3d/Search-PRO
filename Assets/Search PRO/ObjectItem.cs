#if UNITY_EDITOR
using UnityEngine;

namespace SearchPRO {
	public class ObjectItem : SearchItem {

		public Object obj;

		public ObjectItem() { }

		public ObjectItem(Object obj) {
			this.obj = obj;
		}
	}
}
#endif

