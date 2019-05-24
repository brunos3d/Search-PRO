#if UNITY_EDITOR
using System;

namespace SearchPRO {

	[AttributeUsage(AttributeTargets.Method)]
	public class TitleAttribute : Attribute {

		public string title;

		public TitleAttribute(string title) {
			this.title = title;
		}
	}
}
#endif
