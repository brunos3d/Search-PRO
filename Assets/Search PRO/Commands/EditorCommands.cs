#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using System;
using System.Text.RegularExpressions;

namespace SearchPRO {
	public static class EditorCommands {

		[Command]
		[Category("Editor")]
		[Title("Play")]
		[Description("Open a new GameView window.")]
		[Tags("OGV")]
		public static void OpenGameView() {
			EditorWindow editor = (EditorWindow)EditorWindow.CreateInstance(typeof(Editor).Assembly.GetType("UnityEditor.GameView"));
			editor.Show();
		}

		[Command]
		[Category("Open Window")]
		[Title("Open ProjectBrowser")]
		[Description("Open a new ProjectBrowser window.")]
		[Tags("OPB")]
		public static void OpenProjectBrowser() {
			EditorWindow editor = (EditorWindow)EditorWindow.CreateInstance(typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser"));
			editor.Show();
		}

		[Command]
		[Category("Open Window")]
		[Title("Open ConsoleWindow")]
		[Description("Open a new ConsoleWindow window.")]
		[Tags("OCW")]
		public static void OpenConsoleWindow() {
			EditorWindow editor = (EditorWindow)EditorWindow.CreateInstance(typeof(Editor).Assembly.GetType("UnityEditor.ConsoleWindow"));
			editor.Show();
		}
	}
}

#endif
