#if UNITY_EDITOR
using System;

namespace SearchPRO {
	public class ComponentItem : SearchItem {

		public Type type;

		public ComponentItem() { }

		public ComponentItem(Type type) {
			this.type = type;
		}
	}
}
#endif

