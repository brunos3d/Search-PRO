using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace SearchPRO {
	public class TreeNode<T> : IEnumerable {

		public string[] tags;

		public string title;
		public Texture thumb;
		public string description;

		private GUIContent m_content;

		public GUIContent content {
			get {
				return m_content ?? (m_content = new GUIContent(title, thumb, description));
			}
			set {
				this.m_content = value;
				this.title = value.text;
				this.thumb = value.image;
				this.description = value.tooltip;
			}
		}

		public T data;

		public TreeNode<T> parent;

		public List<TreeNode<T>> all_children;

		public List<TreeNode<T>> children_index;

		public Dictionary<string, TreeNode<T>> children;

		public bool isSearch {
			get {
				return content.text == "#Search";
			}
		}

		public bool isRoot {
			get { return parent == null; }
		}

		public bool isLeaf {
			get { return children.Count == 0; }
		}

		public int level {
			get {
				if (this.isRoot) {
					return 0;
				}
				return parent.level + 1;
			}
		}

		public string path {
			get {
				if (this.isRoot) {
					return contentName;
				}
				return parent.path + "/" + contentName;
			}
		}

		public string contentName {
			get {
				return content.text + ":" + content.tooltip;
			}
		}

		public int Count {
			get {
				return children.Count;
			}
		}

		public TreeNode<T> this[int index] {
			get {
				return children_index[index];
			}
		}

		public IEnumerator GetEnumerator() {
			return children.GetEnumerator();
		}

		public TreeNode() { }

		public TreeNode(GUIContent content) {
			this.content = content;
			this.children = new Dictionary<string, TreeNode<T>>();
			this.all_children = new List<TreeNode<T>>();
			this.children_index = new List<TreeNode<T>>();
		}

		public TreeNode(GUIContent content, T data) {
			this.content = content;
			this.data = data;
			this.children = new Dictionary<string, TreeNode<T>>();
			this.all_children = new List<TreeNode<T>>();
			this.children_index = new List<TreeNode<T>>();
		}

		public TreeNode(GUIContent content, T data, params string[] tags) {
			this.content = content;
			this.data = data;
			this.children = new Dictionary<string, TreeNode<T>>();
			this.all_children = new List<TreeNode<T>>();
			this.children_index = new List<TreeNode<T>>();
			this.tags = tags;
		}

		public void AddAnExistingTreeNode(TreeNode<T> tree) {
			this.children[tree.contentName] = tree;
			children_index.Add(tree);
			this.RegisterChildForSearch(tree);
		}

		public TreeNode<T> AddChild(GUIContent content, T child, params string[] tags) {
			TreeNode<T> child_node;
			if (this.children.TryGetValue(content.text + ":" + content.tooltip, out child_node)) {
				return child_node;
			}
			child_node = new TreeNode<T>(content, child, tags) { parent = this };
			this.children[child_node.contentName] = child_node;
			children_index.Add(child_node);
			this.RegisterChildForSearch(child_node);
			return child_node;
		}

		public TreeNode<T> AddChildByPath(GUIContent content, T data, params string[] tags) {
			string path = content.text;
			var paths = path.Split('/').Where(s => !s.IsNullOrWhiteSpace());
			string root_path = paths.ElementAt(0);
			//string end_path = paths[paths.Count - 1];
			// root_path == paths.ElementAt(path_count - 1)
			TreeNode<T> child_node;
			if (paths.Count() == 1) {
				child_node = AddChild(content, data, tags);
				return this.children[child_node.contentName] = child_node;
			}
			else {
				//1/2/3
				//1/2/3/4/5
				string subpath = path.Substring(root_path.Length + 1);
				GUIContent subcontent = new GUIContent(subpath, content.image, content.tooltip);
				if (children.TryGetValue(root_path + ":" + content.tooltip, out child_node)) {
					return child_node.AddChildByPath(subcontent, data);
				}
				else {
					child_node = AddChild(new GUIContent(root_path, content.image), default(T));
					return child_node.AddChildByPath(subcontent, data, tags);
				}
			}
		}

		private void RegisterChildForSearch(TreeNode<T> child) {
			all_children.Add(child);
			if (parent != null) {
				parent.RegisterChildForSearch(child);
			}
		}

		public TreeNode<T> FindTreeNode(Func<TreeNode<T>, bool> predicate) {
			return this.all_children.FirstOrDefault(predicate);
		}

		public TreeNode<T> GetChildren() {
			TreeNode<T> result_tree = new TreeNode<T>(new GUIContent("#Search"), default(T));
			foreach (TreeNode<T> child in this.children.Values) {
				result_tree.AddAnExistingTreeNode(child);
			}
			return result_tree;
		}

		public TreeNode<T> GetAllChildren() {
			TreeNode<T> result_tree = new TreeNode<T>(new GUIContent("#Search"), default(T));
			foreach (TreeNode<T> child in this.all_children) {
				result_tree.AddAnExistingTreeNode(child);
			}
			return result_tree;
		}

		public TreeNode<T> GetTreeNodeInChildren(Func<TreeNode<T>, bool> predicate) {
			TreeNode<T> result_tree = new TreeNode<T>(new GUIContent("#Search"), default(T));
			foreach (TreeNode<T> child in this.children.Values.Where(predicate)) {
				result_tree.AddAnExistingTreeNode(child);
			}
			return result_tree;
		}

		public TreeNode<T> GetTreeNodeInAllChildren(Func<TreeNode<T>, bool> predicate) {
			TreeNode<T> result_tree = new TreeNode<T>(new GUIContent("#Search"), default(T));
			foreach (TreeNode<T> child in this.all_children.Where(predicate)) {
				result_tree.AddAnExistingTreeNode(child);
			}
			return result_tree;
		}
	}
}
