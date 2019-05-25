using System;
using UnityEngine;

namespace SearchPRO {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class IconAttribute : Attribute {

		private Texture m_icon;

		public Type type;

		public string path;

		public Texture icon {
			get {
#if UNITY_EDITOR
				return m_icon ?? (m_icon = UnityEditor.EditorGUIUtility.ObjectContent(null, type).image) ?? (m_icon = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/" + path));
#else
				return m_icon;
#endif
			}
		}

		public IconAttribute(Type type) {
			this.type = type;
		}

		public IconAttribute(string path) {
			this.path = path;
		}
	}
}
