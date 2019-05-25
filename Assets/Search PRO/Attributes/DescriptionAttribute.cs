using System;

namespace SearchPRO {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class DescriptionAttribute : Attribute {

		public string description;

		public DescriptionAttribute(string description) {
			this.description = description;
		}
	}
}
