using System;

namespace SearchPRO {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class CategoryAttribute : Attribute {

		public string category;

		public CategoryAttribute(string category) {
			this.category = category;
		}
	}
}
