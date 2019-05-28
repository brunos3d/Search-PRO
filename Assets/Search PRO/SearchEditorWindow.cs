#if UNITY_EDITOR
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace SearchPRO {
	public class SearchEditorWindow : EditorWindow {

		private class Styles {

			public readonly Texture search_icon;

			public readonly GUIStyle window_background = (GUIStyle)"grey_border";

			public readonly GUIStyle label = EditorStyles.label;

			public readonly GUIStyle scroll_shadow;

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

				scroll_shadow = GlobalSkin.scrollShadow;

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


		private static Styles styles;


		private const float WINDOW_HEAD_HEIGHT = 80.0f;

		private const float WINDOW_FOOT_OFFSET = 10.0f;

		private const string PREFS_ENABLE_TAGS = "SearchPRO: SEW EnableTags Toggle";

		private const string PREFS_ELEMENT_SIZE_SLIDER = "SearchPRO: SEW ElementSize Slider";

		private readonly Color FOCUS_COLOR = new Color32(62, 125, 231, 255);

		private readonly Color STRIP_COLOR_DARK = new Color32(41, 41, 41, 255);

		private readonly Color STRIP_COLOR_LIGHT = new Color32(162, 162, 162, 255);

		private readonly Color WINDOW_HEAD_COLOR_DARK = new Color32(41, 41, 41, 255);

		private readonly Color WINDOW_HEAD_COLOR_LIGHT = new Color32(162, 162, 162, 255);


		private string search;

		private string new_search;

		private bool need_refocus;

		private bool drag_scroll;

		private bool drag_item;

		private bool enable_already;

		private bool enable_layout;

		private bool enable_scroll;

		private float scroll_pos;

		private float scroll_last_pos;

		private float element_list_height = 35;

		private int selected_index;

		private SearchItem selected_item;

		private List<SearchItem> main_list = new List<SearchItem>();

		private bool enableTags {
			get {
				return EditorPrefs.GetBool(PREFS_ENABLE_TAGS, true);
			}
			set {
				EditorPrefs.SetBool(PREFS_ENABLE_TAGS, value);
			}
		}

		private float sliderValue {
			get {
				return EditorPrefs.GetFloat(PREFS_ELEMENT_SIZE_SLIDER, 35);
			}
			set {
				EditorPrefs.SetFloat(PREFS_ELEMENT_SIZE_SLIDER, value);
			}
		}

		private int viewElementCapacity {
			get {
				return (int)((position.height - (WINDOW_HEAD_HEIGHT + (WINDOW_FOOT_OFFSET * 2))) / element_list_height) + 2;
			}
		}

		private bool hasSearch {
			get {
				return !search.IsNullOrEmpty();
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

				element_list_height = sliderValue;

				for (int id = 0; id < 100; id++) {
					string id_string = id.ToString();
					main_list.Add(new SearchItem(
						() => {
							//Debug.Log("Title Loaded");
							return id_string;
						},
						() => {
							//Debug.Log("Icon Loaded");
							return EditorGUIUtility.ObjectContent(null, typeof(Rigidbody)).image;
						},
						() => {
							//Debug.Log("Description Loaded");
							return "My important description";
						},
						() => {
							//Debug.Log("Tags Loaded");
							return new string[3] { "My", "important", "tag" };
						}
					));
				}

				drag_item = false;
				drag_scroll = false;
				need_refocus = true;
				enable_already = true;
			}
		}

		void OnDisable() {
			//Unity bug fix
			Undo.undoRedoPerformed -= Close;
		}

		void OnInspectorUpdate() {
			if ((!(drag_item && DragAndDrop.objectReferences.Length > 0) && focusedWindow != this) || EditorApplication.isCompiling) {
				this.Close();
			}
		}

		void OnGUI() {
			if (!enable_already) {
				OnEnable();
			}

			GUI.Box(new Rect(0.0f, 0.0f, base.position.width, base.position.height), GUIContent.none, styles.window_background);

			if (EditorGUIUtility.isProSkin) {
				EditorGUI.DrawRect(new Rect(1.0f, 1.0f, base.position.width - 2.0f, WINDOW_HEAD_HEIGHT - 1.0f), WINDOW_HEAD_COLOR_DARK);
			}
			else {
				EditorGUI.DrawRect(new Rect(1.0f, 1.0f, base.position.width - 2.0f, WINDOW_HEAD_HEIGHT - 1.0f), WINDOW_HEAD_COLOR_LIGHT);
			}

			KeyboardInputGUI();
			RefreshSearchControl();

			Rect search_rect = new Rect(15.0f, 10.0f, position.width - 30.0f, 30.0f);
			Rect search_icon_rect = new Rect(20.0f, 13.0f, 23.0f, 23.0f);

			GUISkin gui_skin = GUI.skin;
			GUI.skin = GlobalSkin.skin;
			// Search Bar 
			GUI.SetNextControlName("GUIControlSearchBoxTextField");
			new_search = GUI.TextField(search_rect, new_search, styles.search_bar);
			GUI.skin = gui_skin;

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

			int list_items_count = main_list == null ? 0 : main_list.Count;

			enable_scroll = viewElementCapacity < list_items_count;

			Rect list_area = new Rect(1.0f, WINDOW_HEAD_HEIGHT, position.width - (enable_scroll ? 19.0f : 2.0f), position.height - (WINDOW_HEAD_HEIGHT + WINDOW_FOOT_OFFSET));

			if (enable_scroll) {
				scroll_pos = GUI.VerticalScrollbar(new Rect(position.width - 17.0f, WINDOW_HEAD_HEIGHT, 20.0f, list_area.height), scroll_pos, 1.0f, 0.0f, list_items_count - viewElementCapacity + 3);
			}
			else {
				scroll_pos = 0.0f;
			}
			scroll_pos = Mathf.Clamp(scroll_pos, 0.0f, list_items_count);

			if (GUI.Toggle(new Rect(position.width - 60.0f, 55.0f, 50.0f, 20.0f), enableTags, "Tags") != enableTags) {
				enableTags = !enableTags;
			}

			sliderValue = Mathf.Round(GUI.HorizontalSlider(new Rect(position.width - 60.0f, 40.0f, 50.0f, 20.0f), sliderValue, 25, 50) / 5) * 5;

			GUI.BeginClip(list_area);

			PreInputGUI();

			int first_scroll_index = (int)Mathf.Max(scroll_pos, 0);
			int last_scroll_index = (int)Mathf.Min(scroll_pos + viewElementCapacity, list_items_count);

			int draw_index = 0;
			for (int id = first_scroll_index; id < last_scroll_index; id++) {
				bool selected = false;

				SearchItem item = main_list[id];
				float smooth_offset = (scroll_pos - id) * element_list_height;

				Rect layout_rect = new Rect(0.0f, draw_index - smooth_offset, list_area.width, element_list_height);
				if (id % 2 == 1) {
					if (EditorGUIUtility.isProSkin) {
						EditorGUI.DrawRect(layout_rect, STRIP_COLOR_DARK);
					}
					else {
						EditorGUI.DrawRect(layout_rect, STRIP_COLOR_LIGHT);
					}
				}

				//Draw Selection Box
				if (selected_index == draw_index + first_scroll_index || (Event.current.type == EventType.MouseMove && layout_rect.Contains(Event.current.mousePosition))) {
					selected = true;
					selected_item = item;
					selected_index = draw_index + first_scroll_index;
					EditorGUI.DrawRect(layout_rect, FOCUS_COLOR);
				}

				if (enable_layout) {
					string[] tags = item.LoadTags();
					if (enableTags && tags != null && tags.Length > 0) {
						//Draw Tag Buttons
						GUILayout.BeginArea(new Rect(layout_rect.x, layout_rect.y + 5.0f, layout_rect.width, layout_rect.height));
						GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						foreach (string tag in tags) {
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
				if (DrawElementList(layout_rect, item.LoadContent(), selected)) {
					GoToNode(item);
					break;
				}
				draw_index++;
			}
			PostInputGUI();

			GUI.EndClip();

			if (enable_scroll && scroll_pos != 0.0f) {
				Color gui_color = GUI.color;
				if (scroll_pos < 1.0f) {
					GUI.color = new Color(gui_color.r, gui_color.g, gui_color.b, gui_color.a * scroll_pos);
				}
				GUI.Box(new Rect(0.0f, WINDOW_HEAD_HEIGHT, position.width - 15.0f, 10.0f), GUIContent.none, styles.scroll_shadow);
				GUI.color = gui_color;
			}

			// se a distancia entre a ultima atualizacao de itens
			// e a exibicao atual for maior que 1 item
			// basicamente sempre que voce mover 1 item para cima ou para baixo
			if (scroll_last_pos != Mathf.Round(scroll_pos)) {
				// se TODOS os itens exibidos na tela anteriormente
				// nao estiverem mais sendo utilizados
				if (Mathf.Abs(scroll_last_pos - scroll_pos) > viewElementCapacity) {
					//Debug.Log("pass 1");
					int first = (int)Mathf.Max(scroll_last_pos, 0);
					int last = (int)Mathf.Min(scroll_last_pos + viewElementCapacity, list_items_count);

					for (int id = first; id < last; id++) {
						//Debug.Log(id);
						main_list[id].Unload();
					}
				}
				else {
					//Debug.Log("pass 2");
					// se o scroll descer 3 itens
					// os itens anteriores serao despachados
					if (scroll_pos > scroll_last_pos) {
						//Debug.Log("desceu");
						int first = (int)Mathf.Max(scroll_last_pos, 0);
						int last = (int)Mathf.Min(scroll_pos, list_items_count);

						for (int id = first; id < last; id++) {
							main_list[id].Unload();
						}
					}
					// se o scroll subir 3 itens
					// os itens posteriores serao despachados
					else if (scroll_pos < scroll_last_pos) {
						//Debug.Log("subiu");
						int first = (int)Mathf.Max(scroll_pos + viewElementCapacity, 0);
						int last = (int)Mathf.Min(scroll_last_pos + viewElementCapacity, list_items_count);

						for (int id = first; id < last; id++) {
							main_list[id].Unload();
						}
					}
					scroll_last_pos = Mathf.Round(scroll_pos);
				}
				RecalculateSize();
			}

			if (Event.current.type == EventType.Repaint) {
				enable_layout = true;
			}
			Repaint();
		}

		void GoToNode(SearchItem item) {
			if (item == null) return;
			item.Execute();
			this.Close();
			return;
		}

		void RefreshSearchControl() {
			if (search != new_search) {
				if (new_search.IsNullOrEmpty()) {
				}
				else {
					string new_search_escape = Regex.Escape(new_search);
					main_list = main_list.Where(item =>
					Regex.IsMatch(item.LoadTitle(), new_search_escape, RegexOptions.IgnoreCase)
						|| Regex.IsMatch(item.LoadDescription(), new_search_escape, RegexOptions.IgnoreCase)
						|| (enableTags && item.LoadTags().Any(tag => Regex.IsMatch(tag, new_search_escape, RegexOptions.IgnoreCase)))).ToList();

				}
				search = new_search;
			}
		}

		public string HighlightText(string text, string format) {
			if (text.IsNullOrEmpty() || format.IsNullOrEmpty()) return text;
			return Regex.Replace(text, Regex.Escape(format), (match) => string.Format("<color=#FFFF00><b>{0}</b></color>", match), RegexOptions.IgnoreCase);
		}

		public bool DrawElementList(Rect layout_rect, GUIContent content, bool selected) {
			bool trigger = false;
			Event current = Event.current;

			// My custom button =]
			if (current.type == EventType.MouseUp) {
				if (layout_rect.Contains(current.mousePosition)) {
					trigger = true;
					current.Use();
				}
			}

			Rect icon_rect = new Rect(layout_rect.x + 10.0f, layout_rect.y, element_list_height, element_list_height);
			Rect title_rect = new Rect(element_list_height + 5.0f, layout_rect.y, layout_rect.width - element_list_height - 10.0f, layout_rect.height);
			Rect subtitle_rect = new Rect(title_rect);

			GUI.Label(icon_rect, content.image, styles.search_icon_item);
			if (!search.IsNullOrEmpty()) {
				string title = HighlightText(content.text, search);
				EditorGUI.LabelField(title_rect, title, selected ? styles.on_search_title_item : styles.search_title_item);

				if (element_list_height > 30) {
					string subtitle = HighlightText(content.tooltip, search);
					//string subtitle = content.tooltip.Replace(search, string.Format("<color=#ffff00ff><b>{0}</b></color>", search));
					EditorGUI.LabelField(subtitle_rect, subtitle, selected ? styles.on_search_description_item : styles.search_description_item);
				}
			}
			else {
				EditorGUI.LabelField(title_rect, content.text, selected ? styles.on_search_title_item : styles.search_title_item);
				if (element_list_height > 30) {
					EditorGUI.LabelField(subtitle_rect, content.tooltip, selected ? styles.on_search_description_item : styles.search_description_item);
				}
			}

			return !drag_scroll && !drag_item && trigger;
		}

		void PreInputGUI() {
			Event current = Event.current;

			switch (current.type) {
				case EventType.MouseDown:
				drag_item = false;
				drag_scroll = false;
				break;
				case EventType.ScrollWheel:
				drag_item = false;
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
				drag_item = false;
				break;
				case EventType.MouseUp:
				drag_item = false;
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
					if (current.keyCode == KeyCode.Home) {
						selected_index = 0;
						scroll_pos = 0.0f;
						current.Use();
					}
					else if (current.keyCode == KeyCode.End) {
						selected_index = main_list.Count - 1;
						scroll_pos = main_list.Count;
						current.Use();
					}
					else if (current.keyCode == KeyCode.PageDown) {
						selected_index += viewElementCapacity;
						scroll_pos += viewElementCapacity;
						if (selected_index >= main_list.Count) {
							selected_index = 0;
							scroll_pos = 0.0f;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.PageUp) {
						selected_index -= viewElementCapacity;
						scroll_pos -= viewElementCapacity;
						if (selected_index < 0) {
							selected_index = main_list.Count - 1;
							scroll_pos = main_list.Count;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.DownArrow) {
						selected_index++;
						if (selected_index >= scroll_pos + viewElementCapacity - 2) {
							scroll_pos++;
						}
						if (selected_index >= main_list.Count) {
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
							selected_index = main_list.Count - 1;
							scroll_pos = main_list.Count;
						}
						current.Use();
					}
					else if ((current.keyCode == KeyCode.Return) || (current.keyCode == KeyCode.KeypadEnter)) {
						this.GoToNode(selected_item);
					}
					//}
				}
				break;
			}
		}

		void RecalculateSize() {
			//enable_layout = false;
			float width = 0.0f;
			int list_items_count = main_list == null ? 0 : main_list.Count;
			int first_scroll_index = (int)Mathf.Max(scroll_pos, 0);
			int last_scroll_index = (int)Mathf.Min(scroll_pos + viewElementCapacity, list_items_count);

			for (int id = first_scroll_index; id < last_scroll_index; id++) {
				SearchItem item = main_list[id];
				float tags_width = 0.0f;
				string[] tags = item.LoadTags();
				if (enableTags && tags != null && tags.Length > 0) {
					foreach (string tag in tags) {
						tags_width += GUIUtils.GetTextWidth(tag, styles.tag_button) + 5.0f;
					}
				}
				width = Mathf.Max(width, GUIUtils.GetTextWidth(item.LoadTitle(), styles.search_title_item) + 85.0f + tags_width);
				width = Mathf.Max(width, GUIUtils.GetTextWidth(item.LoadDescription(), styles.search_description_item) + 85.0f);
			}
			width = Mathf.Max(Screen.currentResolution.width / 2.0f, width);
			Vector2 pos = new Vector2(Screen.currentResolution.width / 2.0f - width / 2.0f, 100.0f);
			Vector2 size = new Vector2(width, Mathf.Min(WINDOW_HEAD_HEIGHT + (main_list.Count * element_list_height) + (WINDOW_FOOT_OFFSET * 2.0f), Screen.currentResolution.height - pos.y - 150.0f));

			position = new Rect(pos, size);
		}
	}
}
#endif

