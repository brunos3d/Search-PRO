#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	public static class CreationCommands {

		[Command]
		[Category("Create")]
		[Title("Create Empty")]
		[Description("Create an empty GameObject.")]
		public static void CreateEmpty() {
			UnityObject instance = new GameObject();
			Undo.RegisterCreatedObjectUndo(instance, "Create Empty Object");
		}

		[Command]
		[Category("Create")]
		[Title("Create Cube")]
		[Description("Create a primitive Cube.")]
		public static void CreateCube() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cube");
		}

		[Command]
		[Category("Create")]
		[Title("Create Sphere")]
		[Description("Create a primitive Sphere.")]
		public static void CreateSphere() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Undo.RegisterCreatedObjectUndo(instance, "Create Sphere");
		}

		[Command]
		[Category("Create")]
		[Title("Create Capsule")]
		[Description("Create a primitive Capsule.")]
		public static void CreateCapsule() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			Undo.RegisterCreatedObjectUndo(instance, "Create Capsule");
		}

		[Command]
		[Category("Create")]
		[Title("Create Plane")]
		[Description("Create a primitive Plane.")]
		public static void CreatePlane() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Plane);
			Undo.RegisterCreatedObjectUndo(instance, "Create Plane");
		}

		[Command]
		[Category("Create")]
		[Title("Create Quad")]
		[Description("Create a primitive Quad.")]
		public static void CreateQuad() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Quad);
			Undo.RegisterCreatedObjectUndo(instance, "Create Quad");
		}

		[Command]
		[Category("Create")]
		[Title("Create Cylinder")]
		[Description("Create a primitive Cylinder.")]
		public static void CreateCylinder() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cylinder");
		}
	}
}

#endif
