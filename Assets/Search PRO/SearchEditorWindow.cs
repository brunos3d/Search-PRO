#if UNITY_EDITOR
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SearchPRO {
	public class SearchEditorWindow : EditorWindow {

		private class Styles {

			public readonly Texture search_icon;

			public readonly GUIStyle window_header = (GUIStyle)"AppToolbar";

			public readonly GUIStyle window_background = (GUIStyle)"grey_border";

			public readonly GUIStyle label = EditorStyles.label;

			public readonly GUIStyle tag_button;

			public readonly GUIStyle search_bar;

			public readonly GUIStyle search_label;

			public readonly GUIStyle search_icon_item;

			public readonly GUIStyle search_title_item;

			public readonly GUIStyle search_description_item;

			public readonly GUIStyle on_search_title_item;

			public readonly GUIStyle on_search_description_item;

			public Styles() {
				search_icon = EditorGUIUtility.FindTexture("Search Icon");

				tag_button = new GUIStyle(EditorStyles.miniButton);
				tag_button.richText = true;

				search_bar = GlobalSkin.searchBar;
				search_label = GlobalSkin.searchLabel;
				search_icon_item = GlobalSkin.searchIconItem;
				search_title_item = GlobalSkin.searchTitleItem;
				search_description_item = GlobalSkin.searchDescriptionItem;

				on_search_title_item = new GUIStyle(GlobalSkin.searchTitleItem);
				on_search_description_item = new GUIStyle(GlobalSkin.searchDescriptionItem);

				on_search_title_item.normal = GlobalSkin.searchTitleItem.onNormal;
				on_search_description_item.normal = GlobalSkin.searchDescriptionItem.onNormal;

				on_search_title_item.hover = GlobalSkin.searchTitleItem.onHover;
				on_search_description_item.hover = GlobalSkin.searchDescriptionItem.onHover;
			}
		}

		public class SearchItem {

			public Texture icon;

			public string title;

			public string description;

			public int object_id;

			public string[] tags;

			public GUIContent content { get; private set; }

			public SearchItem(GUIContent content, int object_id, params string[] tags) {
				this.title = content.text;
				this.description = content.tooltip;
				this.icon = content.image;
				this.object_id = object_id;
				this.tags = tags;
				this.content = new GUIContent(content);
			}

			public SearchItem(string title, string description, int object_id, params string[] tags) {
				this.title = title;
				this.description = description;
				this.object_id = object_id;
				this.tags = tags;
				this.content = new GUIContent(title, description);
			}

			public SearchItem(string title, string description, Texture icon, int object_id, params string[] tags) {
				this.title = title;
				this.description = description;
				this.icon = icon;
				this.object_id = object_id;
				this.tags = tags;
				this.content = new GUIContent(title, icon, description);
			}
		}


		private static Styles styles;


		private const float WINDOW_HEAD_HEIGHT = 80.0f;

		private const float WINDOW_FOOT_OFFSET = 10.0f;

		private const string PREFS_ENABLE_TAGS = "SearchPRO: SEW EnableTags Toggle";

		private const string PREFS_ELEMENT_SIZE_SLIDER = "SearchPRO: SEW ElementSize Slider";

		private readonly Color FOCUS_COLOR = new Color(62.0f / 255.0f, 95.0f / 255.0f, 150.0f / 255.0f);

		private readonly Color STRIP_COLOR_DARK = new Color(0.205f, 0.205f, 0.205f);

		private readonly Color STRIP_COLOR_LIGHT = new Color(0.7f, 0.7f, 0.7f);



		private string search;

		private string new_search;

		private bool need_refocus;

		private bool drag_scroll;

		private bool enable_already;

		private bool enable_layout;

		private bool enable_scroll;

		private float scroll_pos;

		private float element_list_height = 35;

		public int selected_index;

		private int view_element_capacity;


		public List<SearchItem> all_items = new List<SearchItem>();

		public List<SearchItem> search_items = new List<SearchItem>();

		public bool enableTags {
			get {
				return EditorPrefs.GetBool(PREFS_ENABLE_TAGS, true);
			}
			set {
				EditorPrefs.SetBool(PREFS_ENABLE_TAGS, value);
			}
		}

		public float sliderValue {
			get {
				return EditorPrefs.GetFloat(PREFS_ELEMENT_SIZE_SLIDER, 35);
			}
			set {
				EditorPrefs.SetFloat(PREFS_ELEMENT_SIZE_SLIDER, value);
			}
		}

		[MenuItem("Window/Search PRO %SPACE")]
		public static SearchEditorWindow Init() {
			SearchEditorWindow editor = CreateInstance<SearchEditorWindow>();
			editor.wantsMouseMove = true;
			editor.ShowPopup();
			editor.RecalculateSize();
			FocusWindowIfItsOpen<SearchEditorWindow>();
			return editor;
		}

		void OnEnable() {
			if (!enable_already) {
				//Unity bug fix
				Undo.undoRedoPerformed += Close;
				if (styles == null) {
					styles = new Styles();
				}

				need_refocus = true;
				element_list_height = sliderValue;

				foreach (UnityObject obj in Resources.FindObjectsOfTypeAll<UnityObject>()) {
					string[] tags = { "ID: " + obj.GetInstanceID().ToString(), obj.GetType().Name };
					SearchItem new_item = new SearchItem(EditorGUIUtility.ObjectContent(obj, obj.GetType()), obj.GetInstanceID(), tags);
					all_items.Add(new_item);
					search_items.Add(new_item);
				}

				RecalculateSize();
				enable_already = true;
			}
		}

		void OnDisable() {
			//Unity bug fix
			Undo.undoRedoPerformed -= Close;
		}

		void OnGUI() {
			if (focusedWindow != this || EditorApplication.isCompiling) {
				Close();
			}
			if (!enable_already) {
				OnEnable();
			}

			GUI.Box(new Rect(0.0f, 0.0f, base.position.width, base.position.height), GUIContent.none, styles.window_background);

			GUI.Box(new Rect(0.0f, 0.0f, base.position.width, WINDOW_HEAD_HEIGHT), GUIContent.none, styles.window_header);

			view_element_capacity = (int)((position.height - (WINDOW_HEAD_HEIGHT + WINDOW_FOOT_OFFSET)) / element_list_height);

			KeyboardInputGUI();
			RefreshSearchControl();

			Rect search_rect = new Rect(15.0f, 10.0f, position.width - 30.0f, 30.0f);
			Rect search_icon_rect = new Rect(20.0f, 13.0f, 23.0f, 23.0f);

			// Search Bar 
			GUI.SetNextControlName("GUIControlSearchBoxTextField");
			new_search = GUI.TextField(search_rect, new_search, styles.search_bar);

			if (need_refocus) {
				GUI.FocusControl("GUIControlSearchBoxTextField");
				TextEditor txt = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				if (txt != null) {
					txt.MoveLineEnd();
					txt.SelectNone();
				}
				need_refocus = false;
			}

			GUI.DrawTexture(search_icon_rect, styles.search_icon);

			int current_tree_count = search_items == null ? 0 : search_items.Count;

			enable_scroll = view_element_capacity < current_tree_count;

			if (enable_scroll) {
				scroll_pos = GUI.VerticalScrollbar(new Rect(position.width - 17.0f, WINDOW_HEAD_HEIGHT, 20.0f, view_element_capacity * element_list_height), scroll_pos, 1.0f, 0.0f, current_tree_count - view_element_capacity + 1);
			}
			else {
				scroll_pos = 0.0f;
			}
			scroll_pos = Mathf.Clamp(scroll_pos, 0.0f, current_tree_count);

			if (GUI.Toggle(new Rect(position.width - 60.0f, 55.0f, 50.0f, 20.0f), enableTags, "Tags") != enableTags) {
				enableTags = !enableTags;
			}

			sliderValue = Mathf.Round(GUI.HorizontalSlider(new Rect(position.width - 60.0f, 40.0f, 50.0f, 20.0f), sliderValue, 25, 50) / 5) * 5;

			PreInputGUI();

			int first_scroll_index = (int)Mathf.Clamp(scroll_pos, 0, current_tree_count);
			int last_scroll_index = (int)Mathf.Clamp(scroll_pos + view_element_capacity, 0, current_tree_count);

			int draw_index = 0;
			for (int id = first_scroll_index; id < last_scroll_index; id++) {
				bool selected = false;

				SearchItem item = search_items[id];
				Rect layout_rect = new Rect(1.0f, WINDOW_HEAD_HEIGHT + draw_index * element_list_height, position.width - (enable_scroll ? 19.0f : 2.0f), element_list_height);

				if (id % 2 == 1) {
					Rect strip_rect = new Rect(1.0f, WINDOW_HEAD_HEIGHT + draw_index * element_list_height, position.width - (enable_scroll ? 19.0f : 2.0f), element_list_height);

					if (EditorGUIUtility.isProSkin) {
						EditorGUI.DrawRect(strip_rect, STRIP_COLOR_DARK);
					}
					else {
						EditorGUI.DrawRect(strip_rect, STRIP_COLOR_LIGHT);
					}
				}

				//Draw Selection Box
				if (selected_index == draw_index + first_scroll_index || (Event.current.type == EventType.MouseMove && layout_rect.Contains(Event.current.mousePosition))) {
					selected = true;
					//selected_node = node;
					selected_index = draw_index + first_scroll_index;
					EditorGUI.DrawRect(layout_rect, FOCUS_COLOR);
				}

				if (enable_layout) {
					if (enableTags) {
						//Draw Tag Buttons
						GUILayout.BeginArea(new Rect(layout_rect.x, layout_rect.y + 5.0f, layout_rect.width, layout_rect.height));
						GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						foreach (string tag in item.tags) {
							if (GUILayout.Button(HighlightText(tag, search), styles.tag_button, GUILayout.ExpandWidth(false))) {
								new_search = tag;
								need_refocus = true;
							}
						}
						GUILayout.EndHorizontal();
						GUILayout.EndArea();
					}
				}

				//Draw Element Button
				if (DrawElementList(layout_rect, item.content, selected)) {
					//GoToNode(node, true);
					Selection.activeObject = EditorUtility.InstanceIDToObject(item.object_id);
					break;
				}
				draw_index++;
			}
			PostInputGUI();

			if (Event.current.type == EventType.Repaint) {
				enable_layout = true;
			}
			Repaint();
		}

		void RefreshSearchControl() {
			if (search != new_search) {
				if (new_search.IsNullOrEmpty()) {
					search_items = new List<SearchItem>(all_items);
				}
				else {
					search_items.Clear();
					foreach (SearchItem item in all_items) {
						if (Regex.IsMatch(item.title, Regex.Escape(new_search), RegexOptions.IgnoreCase)
							|| Regex.IsMatch(item.description, Regex.Escape(new_search), RegexOptions.IgnoreCase)
							|| (enableTags && item.tags.Any(tag => Regex.IsMatch(tag, Regex.Escape(new_search), RegexOptions.IgnoreCase)))) {
							search_items.Add(item);
						}
					}
				}
				search = new_search;
				RecalculateSize();
			}
		}

		public string HighlightText(string text, string format) {
			if (text.IsNullOrEmpty() || format.IsNullOrEmpty()) return text;
			return Regex.Replace(text, format, (match) => string.Format("<color=#ffff00ff><b>{0}</b></color>", match), RegexOptions.IgnoreCase);
		}

		public bool DrawElementList(Rect rect, GUIContent content, bool selected) {
			Rect layout_rect = new Rect(rect);
			bool trigger = GUI.Button(layout_rect, string.Empty, styles.label);

			Rect icon_rect = new Rect(layout_rect.x + 10.0f, layout_rect.y, element_list_height, element_list_height);
			Rect title_rect = new Rect(element_list_height + 5.0f, layout_rect.y, layout_rect.width - element_list_height - 10.0f, layout_rect.height);
			Rect subtitle_rect = new Rect(title_rect);

			GUI.Label(icon_rect, content.image, styles.search_icon_item);
			if (!search.IsNullOrEmpty()) {
				string title = HighlightText(content.text, search);
				EditorGUI.LabelField(title_rect, title, selected ? styles.on_search_title_item : styles.search_title_item);

				if (sliderValue > 30) {
					string subtitle = HighlightText(content.tooltip, search);
					//string subtitle = content.tooltip.Replace(search, string.Format("<color=#ffff00ff><b>{0}</b></color>", search));
					EditorGUI.LabelField(subtitle_rect, subtitle, selected ? styles.on_search_description_item : styles.search_description_item);
				}
			}
			else {
				EditorGUI.LabelField(title_rect, content.text, selected ? styles.on_search_title_item : styles.search_title_item);
				if (sliderValue > 30) {
					EditorGUI.LabelField(subtitle_rect, content.tooltip, selected ? styles.on_search_description_item : styles.search_description_item);
				}
			}

			return !drag_scroll && trigger;
		}

		void PreInputGUI() {
			Event current = Event.current;

			switch (current.type) {
				case EventType.MouseDown:
				drag_scroll = false;
				break;
				case EventType.ScrollWheel:
				drag_scroll = true;
				scroll_pos += current.delta.y;
				current.Use();
				break;
				case EventType.MouseDrag:
				drag_scroll = true;
				scroll_pos -= current.delta.y / element_list_height;
				current.Use();
				break;
			}
		}

		void PostInputGUI() {
			Event current = Event.current;

			switch (current.type) {
				case EventType.KeyDown:
				break;
				case EventType.MouseUp:
				drag_scroll = false;
				break;
			}
		}

		void KeyboardInputGUI() {
			Event current = Event.current;

			switch (current.type) {
				case EventType.MouseUp:
				if (element_list_height != sliderValue) {
					element_list_height = sliderValue;
					RecalculateSize();
				}
				break;
				case EventType.KeyDown:
				if (current.keyCode == KeyCode.Escape) {
					this.Close();
				}
				if (!current.control) {
					//char current_char = Event.current.character;
					//if (char.IsNumber(current_char)) {
					//	selected_index = (int)(scroll_pos + (char.GetNumericValue(current_char))) - 1;
					//	if (selected_index < 0) {
					//		selected_index = 0;
					//	}
					//	else if (selected_index >= search_items.Count) {
					//		selected_index = search_items.Count - 1;
					//		scroll_pos = search_items.Count;
					//	}
					//	else if (selected_index >= scroll_pos + view_element_capacity) {
					//		scroll_pos += Mathf.Abs(selected_index - view_element_capacity);
					//	}
					//	current.Use();
					//}
					//else {
					if (current.keyCode == KeyCode.Home) {
						selected_index = 0;
						scroll_pos = 0.0f;
						current.Use();
					}
					else if (current.keyCode == KeyCode.End) {
						selected_index = search_items.Count - 1;
						scroll_pos = search_items.Count;
						current.Use();
					}
					else if (current.keyCode == KeyCode.PageDown) {
						selected_index += view_element_capacity;
						scroll_pos += view_element_capacity;
						if (selected_index >= search_items.Count) {
							selected_index = 0;
							scroll_pos = 0.0f;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.PageUp) {
						selected_index -= view_element_capacity;
						scroll_pos -= view_element_capacity;
						if (selected_index < 0) {
							selected_index = search_items.Count - 1;
							scroll_pos = search_items.Count;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.DownArrow) {
						selected_index++;
						if (selected_index >= scroll_pos + view_element_capacity) {
							scroll_pos++;
						}
						if (selected_index >= search_items.Count) {
							selected_index = 0;
							scroll_pos = 0.0f;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.UpArrow) {
						selected_index--;
						if (selected_index < scroll_pos) {
							scroll_pos--;
						}
						if (selected_index < 0) {
							selected_index = search_items.Count - 1;
							scroll_pos = search_items.Count;
						}
						current.Use();
					}
					else if ((current.keyCode == KeyCode.Return) || (current.keyCode == KeyCode.KeypadEnter)) {
					}
					//}
				}
				break;
			}
		}

		void RecalculateSize() {
			enable_layout = false;
			float width = 0.0f;
			foreach (SearchItem item in search_items) {
				float tags_width = 0.0f;
				if (enableTags) {
					foreach (string tag in item.tags) {
						tags_width += GUIUtils.GetTextWidth(tag, styles.tag_button) + 5.0f;
					}
				}
				width = Mathf.Max(width, GUIUtils.GetTextWidth(item.content.text, styles.search_title_item) + 85.0f + tags_width);
			}
			width = Mathf.Max(Screen.currentResolution.width / 2.0f, width);
			Vector2 pos = new Vector2(Screen.currentResolution.width / 2.0f - width / 2.0f, 100.0f);
			Vector2 size = new Vector2(width, Mathf.Min(WINDOW_HEAD_HEIGHT + (search_items.Count * element_list_height) + WINDOW_FOOT_OFFSET, Screen.currentResolution.height - 150.0f));

			position = new Rect(pos, size);
		}
	}
}
#endif

