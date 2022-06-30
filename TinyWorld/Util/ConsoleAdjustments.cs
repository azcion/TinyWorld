using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TinyWorld.Util {

	internal class ConsoleAdjustments : ConsoleUtil {

		private static IntPtr _handle;
		private static int _mode;
		private static Encoding? _encoding;
		private static int _bufferWidth;
		private static int _bufferHeight;
		private static int _windowWidth;
		private static int _windowHeight;

		public static void WriteAllColors () {
			for (int i = 0; i < 256; i++) {
				if (i is > 0 and < 16 && i % 8 == 0) Console.WriteLine();

				Write($"{i,4}", 0, i);

				if (i < 16) {
					if (i == 15) Console.WriteLine();
				} else if ((i - 15) % 36 == 0) {
					Console.WriteLine();
				}
			}
		}

		protected static void PerformConsoleAdjustments () {
			_handle = GetStdHandle(-11);
			GetConsoleMode(_handle, out _mode);
			SetConsoleMode(_handle, _mode | 0x4);
			_encoding = Console.OutputEncoding;
			Console.OutputEncoding = Encoding.UTF8;
			Console.CursorVisible = false;

			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

			Console.Title = "TinyWorld";
			_bufferWidth = Console.BufferWidth;
			_bufferHeight = Console.BufferHeight;
			_windowWidth = Console.WindowWidth;
			_windowHeight = Console.WindowHeight;

			// Remove scrollbars and prevent scrolling
			Console.SetBufferSize(_windowWidth, _windowHeight);
			Console.SetWindowSize(_windowWidth, _windowHeight);
		}

		protected static void RevertConsoleAdjustments () {
			SetConsoleMode(_handle, _mode);
			Console.OutputEncoding = _encoding!;
			Console.CursorVisible = true;

			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

			Console.SetBufferSize(_bufferWidth, _bufferHeight);
			Console.SetWindowSize(_windowWidth, _windowHeight);
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetStdHandle (int handle);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleMode (IntPtr hConsoleHandle, int mode);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool GetConsoleMode (IntPtr handle, out int mode);

		// [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		// private static extern bool GetCurrentConsoleFontEx (IntPtr consoleOutput, bool maximumWindow, ref ConsoleFontInfoEx lpConsoleCurrentFontEx);
		//
		// [DllImport("kernel32.dll", SetLastError = true)]
		// private static extern bool SetCurrentConsoleFontEx (IntPtr consoleOutput, bool maximumWindow, ConsoleFontInfoEx consoleCurrentFontEx);
		//
		// [StructLayout(LayoutKind.Sequential)]
		// private unsafe struct ConsoleFontInfoEx {
		//
		// 	internal uint cbSize;
		// 	internal uint nFont;
		// 	internal Coord dwFontSize;
		// 	internal int FontFamily;
		// 	internal int FontWeight;
		// 	internal fixed char FaceName[LF_FACESIZE];
		//
		// }
		//
		// [StructLayout(LayoutKind.Sequential)]
		// private struct Coord {
		//
		// 	internal short X;
		// 	internal short Y;
		//
		// 	internal Coord (short x, short y) {
		// 		X = x;
		// 		Y = y;
		// 	}
		//
		// }

	}

}