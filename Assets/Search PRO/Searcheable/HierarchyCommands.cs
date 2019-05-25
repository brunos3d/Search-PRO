#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;


namespace SearchPRO {
	public static class HierarchyCommands {

		[Command]
		[Category("Hierarchy")]
		[Title("Create Empty")]
		[Description("Create an empty GameObject.")]
		[Tags("CEG", "Empty")]
		public static void CreateEmpty() {
			UnityObject instance = new GameObject();
			Undo.RegisterCreatedObjectUndo(instance, "Create Empty Object");
		}

		[Command]
		[Category("Hierarchy")]
		[Title("Create Tree")]
		[Description("Create a Tree.")]
		[Tags("CTE", "Tree")]
		public static void CreateTree() {
			EditorApplication.ExecuteMenuItem("GameObject/3D Object/Tree");
		}

		[Command]
		[Category("Hierarchy")]
		[Title("Create Cube")]
		[Description("Create a primitive Cube.")]
		[Tags("CCE", "Primitive", "Cube")]
		public static void CreateCube() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cube");
		}

		[Command]
		[Category("Hierarchy")]
		[Title("Create Sphere")]
		[Description("Create a primitive Sphere.")]
		[Tags("CCS", "Primitive", "Sphere")]
		public static void CreateSphere() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Undo.RegisterCreatedObjectUndo(instance, "Create Sphere");
		}

		[Command]
		[Category("Hierarchy")]
		[Title("Create Capsule")]
		[Description("Create a primitive Capsule.")]
		[Tags("CCC", "Primitive", "Capsule")]
		public static void CreateCapsule() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			Undo.RegisterCreatedObjectUndo(instance, "Create Capsule");
		}

		[Command]
		[Category("Hierarchy")]
		[Title("Create Plane")]
		[Description("Create a primitive Plane.")]
		[Tags("CCP", "Primitive", "Plane")]
		public static void CreatePlane() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Plane);
			Undo.RegisterCreatedObjectUndo(instance, "Create Plane");
		}

		[Command]
		[Category("Hierarchy")]
		[Title("Create Quad")]
		[Description("Create a primitive Quad.")]
		[Tags("CCQ", "Primitive", "Quad")]
		public static void CreateQuad() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Quad);
			Undo.RegisterCreatedObjectUndo(instance, "Create Quad");
		}

		[Command]
		[Category("Hierarchy")]
		[Title("Create Cylinder")]
		[Description("Create a primitive Cylinder.")]
		[Tags("CCY", "Primitive", "Cylinder")]
		public static void CreateCylinder() {
			UnityObject instance = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			Undo.RegisterCreatedObjectUndo(instance, "Create Cylinder");
		}
	}
}

#endif
