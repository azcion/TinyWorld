using System;
using Mathy;

namespace TinyWorld.Util {

	internal class ConsoleUtil {

		protected static void Position (Vec2 vec) {
			Console.SetCursorPosition(vec.X, vec.Y);
		}

		protected static void Position (int left, int top) {
			Console.SetCursorPosition(left, top);
		}

		protected static void Write (string text) {
			Console.Write(text);
		}

		protected static void Write (string text, int foreground, int background) {
			Console.Write($"\x1b[48;5;{background}m\x1b[38;5;{foreground}m{text}\x1b[39;49m");
		}

		protected static void Clear () {
			Console.Write("\x1b[39;49m");
			Console.Clear();
		}

	}

}