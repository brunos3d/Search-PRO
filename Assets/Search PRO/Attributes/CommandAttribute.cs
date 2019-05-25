using System;

namespace SearchPRO {
	[AttributeUsage(AttributeTargets.Method)]
	public class CommandAttribute : Attribute {

		public CategoryAttribute category;
		public IconAttribute icon;
		public TitleAttribute title;
		public DescriptionAttribute description;
		public TagsAttribute tags;

		public CommandAttribute() { }
	}
}
