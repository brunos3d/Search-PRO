#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SearchPRO {
	public static class ReflectionUtils {

		private const BindingFlags METHOD_BIND_FLAGS = BindingFlags.Public | BindingFlags.Static;

		public static IEnumerable<Type> GetTypesFrom(this Assembly assembly) {
			foreach (Type type in assembly.GetTypes()) {
				if (type != null && type.IsPublic) {
					yield return type;
				}
			}
		}

		public static IEnumerable<MethodInfo> GetMethodsFrom(this Type type) {
			foreach (MethodInfo method in type.GetMethods(METHOD_BIND_FLAGS)) {
				yield return method;
			}
		}

		/// <summary>
		/// Returns the first attribute of type "T" of the member or inherited if "inherit" is true
		/// </summary>
		public static T GetAttribute<T>(this MemberInfo member, bool inherit) where T : Attribute {
			return (T)member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
		}

		/// <summary>
		/// Checks whether there is an attribute on a member
		/// </summary>
		public static bool HasAttribute<T>(this MemberInfo member, bool inherit) where T : Attribute {
			return GetAttribute<T>(member, inherit) != null;
		}
	}
}
#endif
