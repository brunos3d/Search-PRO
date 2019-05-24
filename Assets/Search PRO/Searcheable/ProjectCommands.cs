#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityObject = UnityEngine.Object;


namespace SearchPRO {
	public static class ProjectCommands {

		[Command]
		[Category("Project")]
		[Title("Create Folder")]
		[Description("Creates a new folder in the project.")]
		public static void CreateFolder() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Folder");
		}

		[Command]
		[Category("Project")]
		[Title("Create C# Script")]
		[Description("Creates a new C# script in the project.")]
		public static void CreateCSScript() {
			EditorApplication.ExecuteMenuItem("Assets/Create/C# Script");
		}

		[Command]
		[Category("Project/Create Shader")]
		[Title("Standard Surface Shader")]
		[Description("Creates a new Standard Surface Shader in the project.")]
		public static void CreateStandardSurfaceShader() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Standard Surface Shader");
		}

		[Command]
		[Category("Project/Create Shader")]
		[Title("Unlit Shader")]
		[Description("Creates a new Unlit Shader in the project.")]
		public static void CreateUnlitShader() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Unlit Shader");
		}

		[Command]
		[Category("Project/Create Shader")]
		[Title("Image Effect Shader")]
		[Description("Creates a new Image Effect Shader in the project.")]
		public static void CreateImageEffectShader() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Image Effect Shader");
		}

		[Command]
		[Category("Project/Create Shader")]
		[Title("Shader Variant Collection")]
		[Description("Creates a new Shader Variant Collection in the project.")]
		public static void CreateShaderVariantCollection() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Shader/Shader Variant Collection");
		}

		[Command]
		[Category("Project")]
		[Title("Create Shader")]
		[Description("Creates a new Shader in the project.")]
		public static void CreateShader() {
			EditorApplication.ExecuteMenuItem("Assets/Create/Shader");
		}
	}
}

#endif
