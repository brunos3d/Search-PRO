using System;

namespace SearchPRO {
	[AttributeUsage(AttributeTargets.Class)]
	public class SearchInterfaceAttribute : Attribute {

		public CategoryAttribute category;
		public IconAttribute icon;
		public TitleAttribute title;
		public DescriptionAttribute description;
		public TagsAttribute tags;

		public bool close_on_lost_focus;

		public SearchInterfaceAttribute(bool close_on_lost_focus = false) {
			this.close_on_lost_focus = close_on_lost_focus;
		}
	}
}
