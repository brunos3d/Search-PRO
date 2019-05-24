#if UNITY_EDITOR
using System;

namespace SearchPRO {
	[AttributeUsage(AttributeTargets.Method)]
	public class DescriptionAttribute : Attribute {

		public string description;

		public DescriptionAttribute(string description) {
			this.description = description;
		}
	}
}
#endif
