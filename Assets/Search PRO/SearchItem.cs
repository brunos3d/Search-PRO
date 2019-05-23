#if UNITY_EDITOR
using UnityEngine;

namespace SearchPRO {
	public class SearchItem {

		public Texture icon;

		public string title;

		public string description;

		public object data;

		public string[] tags;

		public GUIContent content { get; protected set; }

		public SearchItem() { }

		public SearchItem(GUIContent content, object data, params string[] tags) {
			this.title = content.text;
			this.description = content.tooltip;
			this.icon = content.image;
			this.data = data;
			this.tags = tags;
			this.content = new GUIContent(content);
		}

		public SearchItem(string title, string description, object data, params string[] tags) {
			this.title = title;
			this.description = description;
			this.data = data;
			this.tags = tags;
			this.content = new GUIContent(title, description);
		}

		public SearchItem(string title, string description, Texture icon, object data, params string[] tags) {
			this.title = title;
			this.description = description;
			this.icon = icon;
			this.data = data;
			this.tags = tags;
			this.content = new GUIContent(title, icon, description);
		}
	}
}
#endif

