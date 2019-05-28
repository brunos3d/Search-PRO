#if UNITY_EDITOR
using System;
using UnityEngine;

namespace SearchPRO {
	public class Lazy<T> {

		private T stored_value;

		private Func<T> value_factory;

		public bool IsValueCreated { get; private set; }

		public T Value {
			get {
				if (stored_value != null) {
					return stored_value;
				}
				else {
					return stored_value = value_factory.Invoke();
				}
			}
		}

		private T ReturnDefault() {
			return default(T);
		}

		public Lazy() { }

		public Lazy(Func<T> value_factory) {
			this.value_factory = value_factory ?? ReturnDefault;
		}

		public void Unload() {
			if (stored_value is IDisposable) {
				((IDisposable)stored_value).Dispose();
			}
			stored_value = default(T);
		}
	}

	public class SearchItem {

		private Lazy<Texture> thumb;

		private Lazy<string> title;

		private Lazy<string> description;

		private Lazy<string[]> tags;

		private GUIContent content_cache;

		public virtual void Unload() {
			thumb.Unload();
			title.Unload();
			description.Unload();
			tags.Unload();
			content_cache = null;
		}

		public virtual GUIContent LoadContent() {
			return content_cache != null ? content_cache : (content_cache = new GUIContent(title.Value, thumb.Value, description.Value));
		}

		public virtual Texture LoadThumb() {
			return thumb.Value;
		}

		public virtual string LoadTitle() {
			return title.Value;
		}

		public virtual string LoadDescription() {
			return description.Value;
		}

		public virtual string[] LoadTags() {
			return tags.Value;
		}

		public SearchItem() { }

		public SearchItem(Func<string> loadTitleCallback, Func<Texture> loadThumbCallback = null, Func<string> loadDescriptionCallback = null, Func<string[]> loadTagsCallback = null) {
			title = new Lazy<string>(loadTitleCallback);
			thumb = new Lazy<Texture>(loadThumbCallback);
			description = new Lazy<string>(loadDescriptionCallback);
			tags = new Lazy<string[]>(loadTagsCallback);
		}

		public virtual void Execute() { }
	}
}
#endif

