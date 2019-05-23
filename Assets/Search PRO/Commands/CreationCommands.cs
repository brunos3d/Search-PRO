#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	public static class CreationCommands {

		[Command("Create Cube",
			"Create a primitive cube",
			ValidationMode.EditorCommand)]
		public static void CreateCube() {
			Selection.activeGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		}

		[Command("Create Sphere",
			"Create a primitive sphere",
			ValidationMode.EditorCommand)]
		public static void CreateSphere() {
			Selection.activeGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		}

		[Command("Create Capsule",
			"Create a primitive capsule",
			ValidationMode.EditorCommand)]
		public static void CreateCapsule() {
			Selection.activeGameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		}

		[Command("Create Plane",
			"Create a primitive plane",
			ValidationMode.EditorCommand)]
		public static void CreatePlane() {
			Selection.activeGameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
		}

		[Command("Create Quad",
			"Create a primitive quad",
			ValidationMode.EditorCommand)]
		public static void CreateQuad() {
			Selection.activeGameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
		}

		[Command("Create Cylinder",
			"Create a primitive cylinder",
			ValidationMode.EditorCommand)]
		public static void CreateCylinder() {
			Selection.activeGameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		}
	}
}

#endif
