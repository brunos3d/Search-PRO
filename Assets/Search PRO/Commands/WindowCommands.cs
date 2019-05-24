#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using System;
using System.Text.RegularExpressions;

namespace SearchPRO {
	public static class WindowCommands {

		[Command]
		[Category("Open Window")]
		[Title("Open SceneView")]
		[Description("Open the SceneView window.")]
		[Tags("OSV")]
		public static void OpenSceneView() {
			EditorApplication.ExecuteMenuItem("Window/General/Scene");
		}

		[Command]
		[Category("Open Window")]
		[Title("Open GameView")]
		[Description("Open the GameView window.")]
		[Tags("OGV")]
		public static void OpenGameView() {
			EditorApplication.ExecuteMenuItem("Window/General/Game");
		}

		[Command]
		[Category("Open Window")]
		[Title("Open ProjectBrowser")]
		[Description("Open the ProjectBrowser window.")]
		[Tags("OPB")]
		public static void OpenProjectBrowser() {
			EditorApplication.ExecuteMenuItem("Window/General/Project");
		}

		[Command]
		[Category("Open Window")]
		[Title("Open ConsoleWindow")]
		[Description("Open the Console window.")]
		[Tags("OCW")]
		public static void OpenConsoleWindow() {
			EditorWindow editor = (EditorWindow)EditorWindow.CreateInstance(typeof(Editor).Assembly.GetType("UnityEditor.ConsoleWindow"));
			editor.Show();
		}

		[Command]
		[Category("Open Window")]
		[Title("Open AssetStore")]
		[Description("Open the AssetStore window.")]
		[Tags("OAS")]
		public static void OpenAssetStore() {
			EditorWindow editor = (EditorWindow)EditorWindow.CreateInstance(typeof(Editor).Assembly.GetType("UnityEditor.AssetStoreWindow"));
			editor.Show();
		}
	}
}

#endif
