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

		public CategoryAttribute category;
		public TitleAttribute title;
		public DescriptionAttribute description;
		public TagsAttribute tags;

		public CommandAttribute() { }
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class CategoryAttribute : Attribute {

		public string category;

		public CategoryAttribute(string category) {
			this.category = category;
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class TitleAttribute : Attribute {

		public string title;

		public TitleAttribute(string title) {
			this.title = title;
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class DescriptionAttribute : Attribute {

		public string description;

		public DescriptionAttribute(string description) {
			this.description = description;
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	public class TagsAttribute : Attribute {

		public string[] tags;

		public TagsAttribute(params string[] tags) {
			this.tags = tags;
		}
	}
}
#endif
