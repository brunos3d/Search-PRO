#if UNITY_EDITOR
using System;

namespace SearchPRO {

	public enum Validation {
		None,
		searchInput,
		activeObject,
		activeGameObject,
		activeTransform,
		objects,
		gameObjects,
		transforms,
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class CommandAttribute : Attribute {

		public string title;

		public string description;

		public string[] tags;

		public Validation validation;

		public CommandAttribute(string title, string description, params string[] tags) {
			this.tags = tags;
			this.title = title;
			this.description = description;
			this.validation = Validation.None;
		}

		public CommandAttribute(string title, string description, Validation validation, params string[] tags) {
			this.tags = tags;
			this.title = title;
			this.validation = validation;
			this.description = description;
		}
	}
}
#endif
