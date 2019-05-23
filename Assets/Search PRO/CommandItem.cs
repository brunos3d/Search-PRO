﻿#if UNITY_EDITOR
using UnityEngine;
using System.Reflection;

namespace SearchPRO {
	public class CommandItem : SearchItem {

		public CommandAttribute flag;

		public MethodInfo method {
			get {
				return (MethodInfo)base.data;
			}
			private set {
				base.data = value;
			}
		}

		public CommandItem(CommandAttribute flag, MethodInfo method, Validation validation) {
			this.flag = flag;

			flag.validation = validation;

			this.method = method;
			base.tags = flag.tags;
			base.title = flag.title;
			base.description = flag.description;
			base.content = new GUIContent(flag.title, flag.description);
		}
	}
}
#endif
