﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	public static class TransformCommands {

		[Command]
		[Category("Transform")]
		[Title("Reset Position")]
		[Description("Moves the selected object to zero position.")]
		public static void ResetPosition(Transform[] transforms) {
			Undo.RecordObjects(transforms, "Reset Position");
			foreach (var tr in transforms) {
				tr.position = Vector3.zero;
			}
		}

		[Command]
		[Category("Transform")]
		[Title("Reset Rotation")]
		[Description("Sets the rotation of the selected object to zero.")]
		public static void ResetRotation(Transform[] transforms) {
			Undo.RecordObjects(transforms, "Reset Rotation");
			foreach (var tr in transforms) {
				tr.rotation = Quaternion.identity;
			}
		}

		[Command]
		[Category("Transform")]
		[Title("Selection LookAt")]
		[Description("Makes all selected objects to look at the last selected object.")]
		public static void SelectionLookAt(Transform[] transforms) {
			Undo.RecordObjects(transforms, "Selection LookAt");
			int last = transforms.Length;
			for (int id = 0; id < last; id++) {
				if (id < last - 1) {
					transforms[id].LookAt(transforms[last - 1]);
				}
			}
		}
	}
}

#endif