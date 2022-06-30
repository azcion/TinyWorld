using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Mathy;
using TinyWorld.Util;

namespace TinyWorld.Builder {

	internal sealed class UIBuilder : ConsoleUtil {

		internal record Config {

			internal Vec2 TitleOrigin { get; init; }
			internal Vec2 InfoOrigin { get; init; }
			internal Vec2 InfoSize { get; init; }
			internal Vec2 MapOrigin { get; init; }
			internal Vec2 MapSize { get; init; }
			internal int WorldHueIndex { get; init; }

		}

		internal UIBuilder (bool sleep, Config config, Rand rand) {
			C = config;
			_sleep = sleep;
			_rand = rand;
		}

		private Config C { get; }

		private readonly bool _sleep;
		private readonly Rand _rand;

		private static readonly string[] TitleLines = {
			"         _ _ _         _   _ ",
			" _____ _| | | |___ ___| |_| |",
			"|_   _|_|___ _|_. |  _| | . |",
			"  | | | |   | | |_|_| |_|___|",
			"  |_| |_|_|_|_  |            ",
			"            |___|            "
		};

		[SuppressMessage("ReSharper", "StringLiteralTypo")]
		private static readonly string[] FormattedTitleLines = {
			"         _ _ _         _   _ ",
			" _____ _|A|A|A|___ ___|A|_|A|",
			"|EDDDE|E|BBBAB|BCA|AAB|A|ACA|",
			"  |D| |D|DDD|D|D|B|B| |B|BBB|",
			"  |E| |E|E|E|EDD|            ",
			"            |EEE|            "
		};

		private static readonly int[] Gradient = {
			 51,  50,  49,  48,  47,  46,  82, 118, 154, 190,
			226, 220, 214, 208, 202, 203, 204, 205, 206, 207,
			201, 200, 199, 198, 197, 196, 160, 124,  88,  52,
			 16,  17,  18,  19,  20,  21,  27 , 33,  39,  45
		};

		private const int TitleTinyColor = 23;
		private const int TitleWorldColor = 22;

		internal void DrawTitleLoading () {
			for (int i = 0; i < TitleLines.Length; i++) {
				Position(C.TitleOrigin.X, C.TitleOrigin.Y + i);
				Write($"\x1b[39;49m{TitleLines[i]}");
			}
		}

		[SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
		internal void DrawTitle () {
			string Colorize (string text, string target, string replacement, int color) {
				return text.Replace(target, $"\x1b[48;5;{color}m{replacement}\x1b[49m");
			}

			for (int i = FormattedTitleLines.Length - 1; i >= 0; i--) {
				Position(C.TitleOrigin.X, C.TitleOrigin.Y + i);
				string line = FormattedTitleLines[i];
				line = Colorize(line, "A", " ", TitleWorldColor);
				line = Colorize(line, "B", "_", TitleWorldColor);
				line = Colorize(line, "C", ".", TitleWorldColor);
				line = Colorize(line, "D", " ", TitleTinyColor);
				line = Colorize(line, "E", "_", TitleTinyColor);
				Write($"\x1b[39;49m{line}");
				Thread.Sleep(_sleep ? 25 : 0);
			}
		}

		internal void DrawUIEdges () {
			// Top line
			Position(C.MapOrigin - 1);
			Write($"\x1b[39;49m╔{new string('═', C.MapSize.X)}╗");

			// Bottom line
			Position(C.MapOrigin.X - 1, C.MapOrigin.Y + C.MapSize.Y);
			Write($"\x1b[39;49m╚{new string('═', C.MapSize.X)}╝");

			// Vertical lines
			for (int i = 0; i < C.MapSize.Y; i++) {
				// Left
				Position(C.MapOrigin.X - 1, C.MapOrigin.Y + i);
				Write("\x1b[39;49m║");

				// Right
				Position(C.MapOrigin.X + C.MapSize.X, C.MapOrigin.Y + i);
				Write("\x1b[39;49m║");
			}
		}

		internal void DrawPreMapImage () {
			for (int y = 0; y < C.MapSize.Y; y++) {
				Position(C.MapOrigin.X, C.MapOrigin.Y + y);

				for (int x = 0; x < C.MapSize.X; x++) {
					int index = (x + y) % Gradient.Length;
					int color = Gradient[index];
					Write($"\x1b[38;5;{color}m͛");
				}
			}
		}

		internal void DisplayInfo () {
			void At (int line) {
				Position(C.InfoOrigin.X, C.InfoOrigin.Y + line);
			}

			int y = 1;
			At(y++);
			Write($"\x1b[39;49mSeed: {_rand.Seed}");
			At(y++);
			Write($"World: {C.WorldHueIndex,-2}");

			y += 2;
			At(y++);
			Write("Commands:");
			At(y++);
			Write("  [A]: previous seed");
			At(y++);
			Write("  [D]: next seed");
			At(y++);
			Write("  [Q]: previous world");
			At(y++);
			Write("  [E]: next world");
			At(y++);
			Write("  [F]: toggle dark");
			At(y++);
			Write("  [F1]: save");
			At(y++);
			Write("  [F2]: load");
		}

		internal void DisplayFrameTime (double frameTime) {
			Position(C.InfoOrigin.X, C.InfoOrigin.Y + C.InfoSize.Y);
			Write($"\x1b[39;49mΔT: {frameTime * 1000}ms     ");
		}

	}

}