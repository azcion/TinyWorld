using System;

namespace TinyWorld.Util {

	internal static class Ticker {

		/// <summary>
		/// Last tick processing time in seconds.
		/// </summary>
		internal static double DeltaTime { get; private set; }

		private static long _lastFrame = Environment.TickCount64;

		internal static void Tick () {
			DeltaTime = (Environment.TickCount64 - _lastFrame) / 1000d;
			_lastFrame = Environment.TickCount64;
		}

	}

}