#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	public static class CreationCommands {

		[Command("Create Cube", "Create a primitive cube")]
		public static void CreateCube() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cube");
		}

		[Command("Create Sphere", "Create a primitive sphere")]
		public static void CreateSphere() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cube");
		}

		[Command("Create Capsule", "Create a primitive capsule")]
		public static void CreateCapsule() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cube");
		}

		[Command("Create Plane", "Create a primitive plane")]
		public static void CreatePlane() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Plane);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cube");
		}

		[Command("Create Quad", "Create a primitive quad")]
		public static void CreateQuad() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Quad);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cube");
		}

		[Command("Create Cylinder", "Create a primitive cylinder")]
		public static void CreateCylinder() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cube");
		}
	}
}

#endif
