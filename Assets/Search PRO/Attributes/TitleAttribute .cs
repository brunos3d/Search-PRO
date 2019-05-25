using System;

namespace SearchPRO {

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class TitleAttribute : Attribute {

		public string title;

		public TitleAttribute(string title) {
			this.title = title;
		}
	}
}
