using Mathy;

namespace TinyWorld.Model {

	internal record Entity {

		internal Rand Rand { get; init; } = null!;

		// Biomes
		internal Biome ThisBiome { get; init; }
		internal Biome[]? SpawnBiomes { get; init; }
		internal Biome[]? TraversalBiomes { get; init; }

		internal bool CanMove { get; init; }

		// Run n-th action frame (higher numbers reduce activity)
		internal bool UseRunNthActionFrame => RunNthActionFrame != null;
		internal int? RunNthActionFrame { get; init; }

		// Background color
		internal bool UseBackgroundColor => BackgroundColor != null;
		internal int? BackgroundColor { get; init; }

		// Background color animation
		internal bool UseBackgroundColorAnimation => BackgroundColorAnimation != null;
		internal int[]? BackgroundColorAnimation { get; init; }

		// Foreground color
		internal bool UseForegroundColor => ForegroundColor != null;
		internal int? ForegroundColor { get; init; }

		// Foreground color array
		internal bool UseForegroundColorArray => ForegroundColorArray != null;
		internal int[]? ForegroundColorArray { get; init; }

		// Foreground color animation
		internal bool UseForegroundColorAnimation => ForegroundColorAnimation != null;
		internal int[]? ForegroundColorAnimation { get; init; }

		// Symbol
		internal bool UseSymbol => Symbol != null;
		internal char? Symbol { get; init; }

		// Random symbol array
		internal bool UseRandomSymbolArray => RandomSymbolArray != null;
		internal char[]? RandomSymbolArray { get; init; }

		// Symbol animation
		internal bool UseSymbolAnimation => SymbolAnimation != null;
		internal char[]? SymbolAnimation { get; init; }

		// Flat symbol
		internal bool UseFlatSymbol => FlatSymbol != null;
		internal char? FlatSymbol { get; init; }

		// Flat symbol ratio
		internal bool UseFlatSymbolRatio => FlatSymbolRatio != null;
		internal double? FlatSymbolRatio { get; init; }

		internal static T GetNext<T> (T[] array, ref int index) {
			index = (index + 1) % array.Length;

			return array[index];
		}

		internal T GetRandom<T> (T[] array) {
			return array[Rand.Next(array.Length)];
		}

	}

}