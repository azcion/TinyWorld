using TinyWorld.Model;

namespace TinyWorld {

	internal class EntityConfig {

		private static readonly Terrain OceanPrefab = new() {
			ThisBiome = Biome.Ocean,
			ForegroundColorArray = new[] { 32, 33, 38, 39 },
			FlatSymbol = ' ',
			RandomSymbolArray = new[] { '͜', '͝' },
			FlatSymbolRatio = .10
		};

		private static readonly Terrain ForestPrefab = new() {
			ThisBiome = Biome.Forest,
			FlatSymbol = ' ',
			RandomSymbolArray = new[] { '.', ',', '`', '\'', '"', '♣', '♠', 'ᴪ' },
			FlatSymbolRatio = .25
		};

		private static readonly Terrain TaigaPrefab = new() {
			ThisBiome = Biome.Taiga,
			ForegroundColorArray = new[] { 22, 23 },
			RandomSymbolArray = new[] { '▲', 'Ѧ', '♠' },
			FlatSymbol = ' ',
			FlatSymbolRatio = .05
		};

		private static readonly Terrain TundraPrefab = new() {
			ThisBiome = Biome.Tundra,
			ForegroundColorArray = new[] { 244, 248 },
			RandomSymbolArray = new[] { '.', '▴' },
			FlatSymbol = ' ',
			FlatSymbolRatio = .30
		};

		private static readonly Terrain MountainPrefab = new() {
			ThisBiome = Biome.Mountain,
			ForegroundColorArray = new[] { 253, 195 },
			RandomSymbolArray = new[] { '▲', '▴' },
			FlatSymbol = ' ',
			FlatSymbolRatio = .35
		};

		protected static readonly Terrain[] Biomes = {
			OceanPrefab with { BackgroundColor = 17 },
			OceanPrefab with { BackgroundColor = 18 },
			OceanPrefab with { BackgroundColor = 19 },
			OceanPrefab with { BackgroundColor = 20 },
			OceanPrefab with { BackgroundColor = 21 },
			ForestPrefab with { BackgroundColor = 22, ForegroundColorArray = new[] { 28, 29 } },
			ForestPrefab with { BackgroundColor = 28, ForegroundColorArray = new[] { 34, 35 } },
			ForestPrefab with { BackgroundColor = 34, ForegroundColorArray = new[] { 70, 71 } },
			ForestPrefab with { BackgroundColor = 70, ForegroundColorArray = new[] { 64, 65 } },
			ForestPrefab with { BackgroundColor = 64, ForegroundColorArray = new[] { 58, 59 } },
			TaigaPrefab with { BackgroundColor = 58 },
			TundraPrefab with { BackgroundColor = 94 },
			MountainPrefab with { BackgroundColor = 244 },
			MountainPrefab with { BackgroundColor = 250 },
			MountainPrefab with { BackgroundColor = 231 }
		};

		private static readonly Creature OceanAnimalPrefab = new() {
			SpawnBiomes = new[] { Biome.Ocean },
			TraversalBiomes = new[] { Biome.Ocean },
			CanMove = true,
			RunNthActionFrame = 2,
			ForegroundColorAnimation = new[] { 43, 41, 38, 36, 38, 41 },
			Symbol = '▸'
		};

		private static readonly Creature ForestAnimalPrefab = new() {
			SpawnBiomes = new[] { Biome.Forest },
			TraversalBiomes = new[] { Biome.Forest },
			CanMove = true,
			RunNthActionFrame = 5,
			ForegroundColorAnimation = new[] { 94, 130, 131, 130 },
			Symbol = 'ᴥ'
		};

		private static readonly Creature MountainAnimalPrefab = new() {
			SpawnBiomes = new[] { Biome.Taiga, Biome.Tundra },
			TraversalBiomes = new[] { Biome.Taiga, Biome.Tundra },
			CanMove = true,
			RunNthActionFrame = 4,
			ForegroundColorAnimation = new[] { 250, 252, 254, 252 },
			Symbol = 'ᴕ'
		};

		protected static readonly Creature[] Creatures = {
			OceanAnimalPrefab with { ForegroundColorAnimation = new[] { 29, 35, 41, 35 } },
			OceanAnimalPrefab with { RunNthActionFrame = 3, Symbol = '◂' },
			ForestAnimalPrefab,
			ForestAnimalPrefab with { RunNthActionFrame = 4 },
			ForestAnimalPrefab with { RunNthActionFrame = 2, Symbol = 'ᵜ' },
			MountainAnimalPrefab,
			MountainAnimalPrefab with { RunNthActionFrame = 3, Symbol = 'ᴽ' }
		};

	}

}