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

		private bool register_undo;

		private float scroll_pos;

		private float element_list_height = 35;

		public int selected_index;

		private int view_element_capacity;

		public TreeNode<SearchItem> root_tree;

		public TreeNode<SearchItem> current_tree;

		public TreeNode<SearchItem> last_tree;

		public TreeNode<SearchItem> selected_node;

		public readonly List<TreeNode<SearchItem>> parents = new List<TreeNode<SearchItem>>();

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

		public bool hasSearch {
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

				need_refocus = true;

				if (styles == null) {
					styles = new Styles();
				}

				element_list_height = sliderValue;
				root_tree = new TreeNode<SearchItem>(new GUIContent("Home"), null);

				// Pega todos os types na assembly Physics
				foreach (Type type in ReflectionUtils.GetTypesFrom(typeof(UnityObject).Assembly)) {
					// Verifica se eh uma sub-classe do tipo Component
					if (type.IsSubclassOf(typeof(Component))) {
						ComponentItem component = new ComponentItem(type);
						string tag = "A" + Char.ToUpper(type.Name[0]) + Char.ToUpper(type.Name[type.Name.Length - 1]);
						Texture icon = EditorGUIUtility.ObjectContent(null, type).image;
						root_tree.AddChildByPath(new GUIContent("Add Component/Add " + type.Name, icon, "Add Component " + type.FullName + " to selected GameObject(s)"), component, tag);
					}
				}
				// Pega todos os types na assembly Physics
				foreach (Type type in ReflectionUtils.GetTypesFrom(typeof(Rigidbody).Assembly)) {
					// Verifica se eh uma sub-classe do tipo Component
					if (type.IsSubclassOf(typeof(Component))) {
						ComponentItem component = new ComponentItem(type);
						string tag = "A" + Char.ToUpper(type.Name[0]) + Char.ToUpper(type.Name[type.Name.Length - 1]);
						Texture icon = EditorGUIUtility.ObjectContent(null, type).image;
						root_tree.AddChildByPath(new GUIContent("Add Component/Add " + type.Name, icon, "Add Component " + type.FullName + " to selected GameObject(s)"), component, tag);
					}
				}

				// Pega todos os types na assembly atual
				foreach (Type type in ReflectionUtils.GetTypesFrom(GetType().Assembly)) {
					// Verifica se eh uma sub-classe do tipo Component
					if (type.IsSubclassOf(typeof(Component))) {
						ComponentItem component = new ComponentItem(type);
						string tag = "A" + Char.ToUpper(type.Name[0]) + Char.ToUpper(type.Name[type.Name.Length - 1]);
						Texture icon = EditorGUIUtility.ObjectContent(null, type).image;
						root_tree.AddChildByPath(new GUIContent("Add Component/Add " + type.Name, icon, "Add Component " + type.FullName + " to selected GameObject(s)"), component, tag);
					}
					// Verifica a existencia da interface
					else if (type.GetInterfaces().Any(i => typeof(ISearchInterface).IsAssignableFrom(i))) {
						SearchInterfaceAttribute t_interface = null;
						IconAttribute t_icon = null;
						CategoryAttribute t_category = null;
						TitleAttribute t_title = null;
						DescriptionAttribute t_description = null;
						TagsAttribute t_tags = null;

						Texture icon = null;
						string category = string.Empty;
						string title = string.Empty;
						string description = string.Empty;
						string[] tags = new string[] { };

						// Verifica a existencia dos attributos
						foreach (Attribute attribute in type.GetCustomAttributes()) {
							if (t_interface == null) {
								if (attribute is SearchInterfaceAttribute) {
									t_interface = (SearchInterfaceAttribute)attribute;
									continue;
								}
							}
							else {
								if (t_icon == null && attribute is IconAttribute) {
									t_icon = (IconAttribute)attribute;
									t_interface.icon = t_icon;
									icon = t_icon.icon;
									continue;
								}
								if (t_category == null && attribute is CategoryAttribute) {
									t_category = (CategoryAttribute)attribute;
									t_interface.category = t_category;
									category = t_category.category;
									continue;
								}
								if (t_title == null && attribute is TitleAttribute) {
									t_title = (TitleAttribute)attribute;
									t_interface.title = t_title;
									title = t_title.title;
									continue;
								}
								if (t_description == null && attribute is DescriptionAttribute) {
									t_description = (DescriptionAttribute)attribute;
									t_interface.description = t_description;
									description = t_description.description;
									continue;
								}
								if (t_tags == null && attribute is TagsAttribute) {
									t_tags = (TagsAttribute)attribute;
									t_interface.tags = t_tags;
									tags = t_tags.tags;
									continue;
								}
							}
						}

						// Cria o item
						if (t_interface != null) {
							ISearchInterface si = (ISearchInterface)Activator.CreateInstance(type);
							if (t_category == null) {
								root_tree.AddChildByPath(new GUIContent(title, icon, description), new InterfaceItem(si, t_interface.close_on_lost_focus), tags);
							}
							else {
								root_tree.AddChildByPath(new GUIContent(string.Format("{0}/{1}", category, title), icon, description), new InterfaceItem(si, t_interface.close_on_lost_focus), tags);
							}
						}

						continue;
					}

					// Pega todos os methods do type atual
					foreach (MethodInfo method in type.GetMethodsFrom()) {
						// Pega e verifica a existencia do attributo
						CommandAttribute m_command = null;
						CategoryAttribute m_category = null;
						IconAttribute m_icon = null;
						TitleAttribute m_title = null;
						DescriptionAttribute m_description = null;
						TagsAttribute m_tags = null;

						Texture icon = null;
						string category = string.Empty;
						string title = string.Empty;
						string description = string.Empty;
						string[] tags = new string[] { };

						foreach (Attribute attribute in method.GetCustomAttributes()) {
							if (m_command == null) {
								if (attribute is CommandAttribute) {
									m_command = (CommandAttribute)attribute;
									continue;
								}
							}
							else {
								if (m_icon == null && attribute is IconAttribute) {
									m_icon = (IconAttribute)attribute;
									m_command.icon = m_icon;
									icon = m_icon.icon;
									continue;
								}
								if (m_category == null && attribute is CategoryAttribute) {
									m_category = (CategoryAttribute)attribute;
									m_command.category = m_category;
									category = m_category.category;
									continue;
								}
								if (m_title == null && attribute is TitleAttribute) {
									m_title = (TitleAttribute)attribute;
									m_command.title = m_title;
									title = m_title.title;
									continue;
								}
								if (m_description == null && attribute is DescriptionAttribute) {
									m_description = (DescriptionAttribute)attribute;
									m_command.description = m_description;
									description = m_description.description;
									continue;
								}
								if (m_tags == null && attribute is TagsAttribute) {
									m_tags = (TagsAttribute)attribute;
									m_command.tags = m_tags;
									tags = m_tags.tags;
									continue;
								}
							}
						}

						if (m_command != null) {
							Validation validation = Validation.None;

							// Realiza a verificacao dos parametros e se o methodo pode ser chamado
							foreach (ParameterInfo param in method.GetParameters()) {
								Type param_type = param.ParameterType;

								// Verifica se o tipo do parametro
								if (typeof(string).IsAssignableFrom(param_type)) {
									validation = Validation.searchInput;
								}
								else if (typeof(GameObject).IsAssignableFrom(param_type)) {
									validation = Validation.activeGameObject;
								}
								else if (typeof(GameObject[]).IsAssignableFrom(param_type)) {
									validation = Validation.gameObjects;
								}
								else if (typeof(Transform).IsAssignableFrom(param_type)) {
									validation = Validation.activeTransform;
								}
								else if (typeof(Transform[]).IsAssignableFrom(param_type)) {
									validation = Validation.transforms;
								}
								else if (typeof(UnityObject).IsAssignableFrom(param_type)) {
									validation = Validation.activeObject;
								}
								else if (typeof(UnityObject[]).IsAssignableFrom(param_type)) {
									validation = Validation.objects;
								}
							}

							if (m_category == null) {
								root_tree.AddChildByPath(new GUIContent(title, icon, description), new CommandItem(m_command, method, validation), tags);
							}
							else {
								root_tree.AddChildByPath(new GUIContent(string.Format("{0}/{1}", category, title), icon, description), new CommandItem(m_command, method, validation), tags);
							}
						}
					}
				}

				GoToHome();
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

			view_element_capacity = (int)((position.height - (WINDOW_HEAD_HEIGHT + (WINDOW_FOOT_OFFSET * 2))) / element_list_height);

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

			if (enable_layout) {
				GUILayout.Space(50.0f);

				GUILayout.BeginHorizontal();
				{
					if (current_tree.isSearch) {
						if (GUILayout.Button(new GUIContent("Home"), GUILayout.ExpandWidth(false), GUILayout.Height(20.0f))) {
							GoToHome();
						}
					}
					for (int id = parents.Count - 1; id >= 0; id--) {
						TreeNode<SearchItem> parent = parents[id];
						if (GUILayout.Button(parent.content.text, GUILayout.ExpandWidth(false), GUILayout.Height(20.0f))) {
							GoToNode(parent, true);
							break;
						}
					}
					GUILayout.Button(current_tree.content.text, GUILayout.ExpandWidth(false), GUILayout.Height(20.0f));
				}
				GUILayout.EndHorizontal();
			}

			int current_tree_count = current_tree == null ? 0 : current_tree.Count;

			enable_scroll = view_element_capacity < current_tree_count;

			Rect list_area = new Rect(1.0f, WINDOW_HEAD_HEIGHT, position.width - (enable_scroll ? 19.0f : 2.0f), position.height - (WINDOW_HEAD_HEIGHT + WINDOW_FOOT_OFFSET));

			if (enable_scroll) {
				scroll_pos = GUI.VerticalScrollbar(new Rect(position.width - 17.0f, WINDOW_HEAD_HEIGHT, 20.0f, list_area.height), scroll_pos, 1.0f, 0.0f, current_tree_count - view_element_capacity + 1);
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

			GUI.BeginClip(list_area);

			int first_scroll_index = (int)Mathf.Clamp(scroll_pos, 0, current_tree_count);
			int last_scroll_index = (int)Mathf.Clamp(scroll_pos + view_element_capacity + 2, 0, current_tree_count);

			int draw_index = 0;
			for (int id = first_scroll_index; id < last_scroll_index; id++) {
				bool selected = false;

				TreeNode<SearchItem> node = current_tree[id];
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
					selected_node = node;
					selected_index = draw_index + first_scroll_index;
					EditorGUI.DrawRect(layout_rect, FOCUS_COLOR);
				}

				if (enable_layout) {
					if (enableTags && node.tags != null && node.tags.Length > 0) {
						//Draw Tag Buttons
						GUILayout.BeginArea(new Rect(layout_rect.x, layout_rect.y + 5.0f, layout_rect.width, layout_rect.height));
						GUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						foreach (string tag in node.tags) {
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
				if (DrawElementList(layout_rect, node.content, selected)) {
					GoToNode(node, true);
					break;
				}
				draw_index++;
			}
			GUI.EndClip();

			PostInputGUI();

			if (enable_scroll && scroll_pos != 0.0f) {
				Color gui_color = GUI.color;
				if (scroll_pos < 1.0f) {
					GUI.color = new Color(gui_color.r, gui_color.g, gui_color.b, gui_color.a * scroll_pos);
				}
				GUI.Box(new Rect(0.0f, WINDOW_HEAD_HEIGHT, position.width - 15.0f, 10.0f), GUIContent.none, styles.scroll_shadow);
				GUI.color = gui_color;
			}

			if (Event.current.type == EventType.Repaint) {
				enable_layout = true;
			}
			Repaint();
		}

		void GoToHome() {
			GoToNode(root_tree, false);
			enable_layout = false;
			new_search = string.Empty;
			need_refocus = true;
		}

		void GoToParent() {
			if (!current_tree.isRoot) {
				GoToNode(current_tree.parent, false);
			}
		}

		void GoToNode(TreeNode<SearchItem> node, bool call_if_is_leaf) {
			if (node == null) return;
			if (node.isLeaf) {
				if (call_if_is_leaf) {
					if (node.data is CommandItem) {
						CommandItem command = (CommandItem)node.data;
						ExecuteCommandItem(command);
					}
					else if (node.data is ComponentItem) {
						ComponentItem component = (ComponentItem)node.data;
						foreach (GameObject go in Selection.gameObjects) {
							if (go.activeInHierarchy && go.hideFlags != HideFlags.NotEditable) {
								Undo.AddComponent(go, component.type);
							}
						}
					}
					else if (node.data is ObjectItem) {
						ObjectItem object_item = (ObjectItem)node.data;
						EditorGUIUtility.PingObject(object_item.obj);
					}
					else if (node.data is InterfaceItem) {
						InterfaceItem interface_item = (InterfaceItem)node.data;
						ContainerWindow.CreateNew((ISearchInterface)interface_item.search_interface, node.content, interface_item.close_on_lost_focus);
					}
					else {
						Debug.Log("No implemented function");
						//Selection.activeObject = EditorUtility.InstanceIDToObject((int)node.data.data);
					}
					this.Close();
				}
			}
			else {
				if (current_tree != null && !current_tree.isSearch) {
					last_tree = current_tree;
				}
				current_tree = node;
				if (last_tree == null) {
					last_tree = current_tree;
				}

				parents.Clear();
				TreeNode<SearchItem> parent = current_tree.parent;
				//while parent isNotRoot
				while (parent != null) {
					parents.Add(parent);
					parent = parent.parent;
				}

				selected_index = 0;
				scroll_pos = 0.0f;
				RecalculateSize();
			}
		}

		void ExecuteCommandItem(CommandItem command) {
			switch (command.validation) {
				default:
				case Validation.None:
				command.method.Invoke(null, null);
				break;

				case Validation.searchInput:
				command.method.Invoke(null, new object[] { search });
				break;

				case Validation.activeGameObject:
				if (Selection.activeGameObject) {
					command.method.Invoke(null, new object[] { Selection.activeGameObject });
				}
				break;

				case Validation.gameObjects:
				if (Selection.gameObjects.Length > 0) {
					command.method.Invoke(null, new object[] { Selection.gameObjects });
				}
				break;

				case Validation.activeTransform:
				if (Selection.activeTransform) {
					command.method.Invoke(null, new object[] { Selection.activeTransform });
				}
				break;

				case Validation.transforms:
				if (Selection.transforms.Length > 0) {
					command.method.Invoke(null, new object[] { Selection.transforms });
				}
				break;

				case Validation.activeObject:
				if (Selection.activeObject) {
					command.method.Invoke(null, new object[] { Selection.activeGameObject });
				}
				break;

				case Validation.objects:
				if (Selection.objects.Length > 0) {
					command.method.Invoke(null, new object[] { Selection.objects });
				}
				break;
			}
		}

		void RefreshSearchControl() {
			if (search != new_search) {
				if (new_search.IsNullOrEmpty()) {
					GoToNode(last_tree, false);
				}
				else {
					string new_search_escape = Regex.Escape(new_search);
					TreeNode<SearchItem> search_result = new TreeNode<SearchItem>(new GUIContent("#Search"));

					foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>().Where(go => go.activeInHierarchy && go.hideFlags != HideFlags.NotEditable && go.hideFlags != HideFlags.HideAndDontSave)) {
						string[] tags = new string[2] { go.tag, LayerMask.LayerToName(go.layer) };
						if (Regex.IsMatch(go.name, new_search_escape, RegexOptions.IgnoreCase)
						|| (enableTags && tags.Any(tag => Regex.IsMatch(tag, new_search_escape, RegexOptions.IgnoreCase)))) {
							ObjectItem item = new ObjectItem(go);
							Texture icon = EditorGUIUtility.ObjectContent(go, typeof(GameObject)).image;
							GUIContent content = new GUIContent(go.name, icon, go.scene.name + "/" + go.name);
							search_result.AddChild(content, item, tags);
						}
					}

					foreach (string path in AssetDatabase.GetAllAssetPaths()) {
						// Ignore built-in packages
						if (path.StartsWith("Packages/")) continue;

						if (Regex.IsMatch(path, new_search_escape, RegexOptions.IgnoreCase)) {
							UnityObject obj = AssetDatabase.LoadAssetAtPath<UnityObject>(path);
							ObjectItem item = new ObjectItem(obj);
							Texture icon = AssetPreview.GetAssetPreview(obj) ?? AssetDatabase.GetCachedIcon(path) ?? AssetPreview.GetMiniThumbnail(obj);
							string tag = obj.GetType().Name;

							GUIContent content;
							if (AssetDatabase.IsValidFolder(path)) {
								content = new GUIContent(obj.name, icon, path);
							}
							else {
								long file_size = new FileInfo(path.Replace(Application.dataPath, "Assets").Replace("\\", "/")).Length;
								string description = string.Format("{0} ({1})", path, EditorUtility.FormatBytes(file_size));
								content = new GUIContent(obj.name, icon, description);
							}
							search_result.AddChild(content, item, tag);
						}
					}

					TreeNode<SearchItem> root_search = root_tree.GetTreeNodeInAllChildren(tn =>

					ValidateItem(tn.data)
					&& Regex.IsMatch(tn.content.text, new_search_escape, RegexOptions.IgnoreCase)
							|| Regex.IsMatch(tn.content.tooltip, new_search_escape, RegexOptions.IgnoreCase)
							|| (enableTags && tn.tags.Any(tag => Regex.IsMatch(tag, new_search_escape, RegexOptions.IgnoreCase))));

					foreach (TreeNode<SearchItem> tree_node in root_search) {
						search_result.AddAnExistingTreeNode(tree_node);
					}

					GoToNode(search_result, false);
				}
				search = new_search;
			}
		}

		bool ValidateItem(SearchItem item) {
			if (item is CommandItem) {
				CommandItem command = (CommandItem)item;
				switch (command.validation) {
					default:
					case Validation.None:
					return true;

					case Validation.activeGameObject:
					if (Selection.activeGameObject) {
						return true;
					}
					return false;

					case Validation.gameObjects:
					if (Selection.gameObjects.Length > 0) {
						return true;
					}
					return false;

					case Validation.activeTransform:
					if (Selection.activeTransform) {
						return true;
					}
					return false;

					case Validation.transforms:
					if (Selection.transforms.Length > 0) {
						return true;
					}
					return false;

					case Validation.activeObject:
					if (Selection.activeObject) {
						return true;
					}
					return false;

					case Validation.objects:
					if (Selection.objects.Length > 0) {
						return true;
					}
					return false;
				}
			}
			return true;
		}

		public string HighlightText(string text, string format) {
			if (text.IsNullOrEmpty() || format.IsNullOrEmpty()) return text;
			return Regex.Replace(text, Regex.Escape(format), (match) => string.Format("<color=#FFFF00><b>{0}</b></color>", match), RegexOptions.IgnoreCase);
		}

		public bool DrawElementList(Rect layout_rect, GUIContent content, bool selected) {
			bool trigger = false;
			Event current = Event.current;

			// My custom button =]
			switch (current.type) {
				case EventType.MouseUp:
				if (layout_rect.Contains(current.mousePosition)) {
					trigger = true;
				}
				break;
			}

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
				if (!drag_scroll && !drag_item && Mathf.Abs(current.delta.x) > 0.8f && Mathf.Abs(current.delta.y) < 0.5f) {
					if (selected_node.data is ObjectItem) {
						DragAndDrop.PrepareStartDrag();
						ObjectItem item = (ObjectItem)selected_node.data;
						DragAndDrop.objectReferences = new UnityObject[] { item.obj };
						DragAndDrop.StartDrag("Dragging " + item.obj.name);
						Event.current.Use();
						drag_item = true;
					}
					else {
						drag_item = false;
						drag_scroll = true;
					}
				}
				if (!drag_item) {
					drag_scroll = true;
					scroll_pos -= current.delta.y / element_list_height;
					current.Use();
				}
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
						selected_index = current_tree.Count - 1;
						scroll_pos = current_tree.Count;
						current.Use();
					}
					else if (current.keyCode == KeyCode.PageDown) {
						selected_index += view_element_capacity;
						scroll_pos += view_element_capacity;
						if (selected_index >= current_tree.Count) {
							selected_index = 0;
							scroll_pos = 0.0f;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.PageUp) {
						selected_index -= view_element_capacity;
						scroll_pos -= view_element_capacity;
						if (selected_index < 0) {
							selected_index = current_tree.Count - 1;
							scroll_pos = current_tree.Count;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.DownArrow) {
						selected_index++;
						if (selected_index >= scroll_pos + view_element_capacity) {
							scroll_pos++;
						}
						if (selected_index >= current_tree.Count) {
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
							selected_index = current_tree.Count - 1;
							scroll_pos = current_tree.Count;
						}
						current.Use();
					}
					else if ((current.keyCode == KeyCode.LeftArrow) || (current.keyCode == KeyCode.Backspace)) {
						if (!hasSearch) {
							this.GoToParent();
							current.Use();
						}
					}
					else if (current.keyCode == KeyCode.RightArrow) {
						if (!hasSearch) {
							this.GoToNode(selected_node, false);
							current.Use();
						}
					}
					else if ((current.keyCode == KeyCode.Return) || (current.keyCode == KeyCode.KeypadEnter)) {
						this.GoToNode(selected_node, true);
					}
					//}
				}
				break;
			}
		}

		void RecalculateSize() {
			enable_layout = false;
			float width = 0.0f;
			foreach (TreeNode<SearchItem> node in current_tree) {
				float tags_width = 0.0f;
				if (enableTags && node.tags != null && node.tags.Length > 0) {
					foreach (string tag in node.tags) {
						tags_width += GUIUtils.GetTextWidth(tag, styles.tag_button) + 5.0f;
					}
				}
				width = Mathf.Max(width, GUIUtils.GetTextWidth(node.content.text, styles.search_title_item) + 85.0f + tags_width);
				width = Mathf.Max(width, GUIUtils.GetTextWidth(node.content.tooltip, styles.search_description_item) + 85.0f);
			}
			width = Mathf.Max(Screen.currentResolution.width / 2.0f, width);
			Vector2 pos = new Vector2(Screen.currentResolution.width / 2.0f - width / 2.0f, 100.0f);
			Vector2 size = new Vector2(width, Mathf.Min(WINDOW_HEAD_HEIGHT + (current_tree.Count * element_list_height) + (WINDOW_FOOT_OFFSET * 2.0f), Screen.currentResolution.height - pos.y - 150.0f));

			position = new Rect(pos, size);
		}
	}
}
#endif

