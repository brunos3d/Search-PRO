#if UNITY_EDITOR
using System;

namespace SearchPRO {
	[AttributeUsage(AttributeTargets.Method)]
	public class TagsAttribute : Attribute {

		public string[] tags;

		public TagsAttribute(params string[] tags) {
			this.tags = tags;
		}
	}
}
#endif
