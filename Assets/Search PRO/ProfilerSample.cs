using System;
using System.Diagnostics;
using System.Text;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace SearchPRO {
	/// <summary>
	/// Prevents wrong profiler samples count.
	/// Very useful for things other than Enhanced Hierarchy, Unity could implement this on its API, just saying :).
	/// </summary>
	public sealed class ProfilerSample : IDisposable {

		private static readonly StringBuilder name = new StringBuilder(150);

		public ProfilerSample(string name, Object targetObject = null) {
			if (!targetObject)
				Profiler.BeginSample(name);
			else
				Profiler.BeginSample(name, targetObject);
		}

		public static ProfilerSample Get() {
			Profiler.BeginSample("Getting Stack Frame");

			var stack = new StackFrame(1, false);

			name.Length = 0;
			name.Append(stack.GetMethod().DeclaringType.Name);
			name.Append(".");
			name.Append(stack.GetMethod().Name);

			Profiler.EndSample();

			return Get(name.ToString(), null);
		}

		public static ProfilerSample Get(string name, Object targetObject = null) {
			return new ProfilerSample(name, targetObject);
		}

		public void Dispose() {
			Profiler.EndSample();
		}
	}
}