#if UNITY_EDITOR
using System;

namespace SearchPRO {

	public enum ValidationMode {
		EditorCommand,
		SelectionIsTypeOf,
		GameObjectHasComponentOfType,
		AtLeastOneGameObject,
		AtLeastTwoGameObjects,
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class CommandAttribute : Attribute {

		public string title;

		public string description;

		public string[] tags;

		public ValidationMode validation;

		public Type explicit_type;

		public CommandAttribute(string title, string description, params string[] tags) {
			this.tags = tags;
			this.title = title;
			this.description = description;
			validation = ValidationMode.EditorCommand;
		}

		public CommandAttribute(string title, string description, ValidationMode validation, params string[] tags) {
			this.tags = tags;
			this.title = title;
			this.validation = validation;
			this.description = description;
		}

		public CommandAttribute(string title, string description, ValidationMode validation, Type explicit_type, params string[] tags) {
			this.tags = tags;
			this.title = title;
			this.validation = validation;
			this.description = description;
			this.explicit_type = explicit_type;
		}
	}
}
#endif
