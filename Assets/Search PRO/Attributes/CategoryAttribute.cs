#if UNITY_EDITOR
using System;

namespace SearchPRO {
	[AttributeUsage(AttributeTargets.Method)]
	public class CategoryAttribute : Attribute {

		public string category;

		public CategoryAttribute(string category) {
			this.category = category;
		}
	}
}
#endif
