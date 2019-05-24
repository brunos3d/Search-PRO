#if UNITY_EDITOR
using UnityEngine;
using System.Reflection;

namespace SearchPRO {
	public class CommandItem : SearchItem {

		public CommandAttribute attribute;

		public MethodInfo method;

		public Validation validation;

		public CommandItem(CommandAttribute attribute, MethodInfo method) {
			this.attribute = attribute;
			this.method = method;
		}

		public CommandItem(CommandAttribute attribute, MethodInfo method, Validation validation) {
			this.attribute = attribute;
			this.method = method;
			this.validation = validation;
		}
	}
}
#endif

