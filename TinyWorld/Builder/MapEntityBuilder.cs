using System.Linq;
using Mathy;
using TinyWorld.Model;

namespace TinyWorld.Builder {

	// todo restructure builder (IMap)
	internal sealed class MapEntityBuilder : EntityConfig {

		internal MapEntityBuilder (IMap builder, Vec2 size, Rand rand) {
			_size = size;
			_builder = builder;
			_rand = rand;
		}

		private readonly Vec2 _size;
		private readonly IMap _builder;
		private readonly Rand _rand;

		internal void FillTerrain (int colorOffset, bool dark) {
			for (int y = 0; y < _size.Y; y++) {
				for (int x = 0; x < _size.X; x++) {
					// Square-ify output coordinates
					int xPos = x - (x % 2 == 1 ? 1 : 0) + 1;
					int yPos = 2 * (y + 1);

					// Get noise value at position
					double noise = Noise.FractalNeg1To1(3, xPos, yPos, _rand.Seed, .5, .02);

					// Normalize to 0-1
					noise = (noise + 1) / 2;

					// Limit to n values
					noise = (int)(noise * Biomes.Length) / (double)Biomes.Length;

					// Map to biome
					int biomeIndex = (int)Smooth.Lerp(0, Biomes.Length, noise);
					Terrain terrain = Biomes[biomeIndex];

					// Color offset
					int backgroundColor = terrain.BackgroundColor + 36 * colorOffset ?? 0;
					backgroundColor %= 256;
					int[]? foregroundColorArray = null;

					if (terrain.UseForegroundColorArray) {
						foregroundColorArray = new int[terrain.ForegroundColorArray!.Length];

						for (int i = 0; i < terrain.ForegroundColorArray!.Length; i++) {
							int foregroundColor = terrain.ForegroundColorArray[i] + 36 * colorOffset;
							foregroundColor %= 256;
							foregroundColorArray[i] = foregroundColor;
						}
					}

					_builder.Place(terrain with {
						Vec = new Vec2(x, y),
						Rand = _rand,
						BackgroundColor = dark ? 0 : backgroundColor,
						ForegroundColorArray = foregroundColorArray ?? terrain.ForegroundColorArray
					});
				}
			}
		}

		internal void FillCreatures (int colorOffset) {
			for (int y = 0; y < _size.Y; y++) {
				for (int x = 0; x < _size.X; x++) {
					if (_rand.NextDouble() <= .925) {
						continue;
					}

					Terrain terrain = _builder.GetTerrainAt(x, y);

					// Reduce spawn chance for non-ocean creatures
					if (terrain.ThisBiome != Biome.Ocean && _rand.NextDouble() <= .775) {
						continue;
					}

					foreach (Creature creature in Creatures) {
						if (creature.SpawnBiomes == null) {
							continue;
						}

						if (!creature.SpawnBiomes.Contains(terrain.ThisBiome)) {
							continue;
						}

						int[]? foregroundColorAnimation = null;

						if (creature.UseForegroundColorAnimation) {
							foregroundColorAnimation = new int[creature.ForegroundColorAnimation!.Length];

							for (int i = 0; i < creature.ForegroundColorAnimation!.Length; i++) {
								int foregroundColor = creature.ForegroundColorAnimation[i] + 36 * colorOffset;
								foregroundColor %= 256;
								foregroundColorAnimation[i] = foregroundColor;
							}
						}

						_builder.Place(creature with {
							Vec = new Vec2(x, y),
							Rand = _rand,
							ForegroundColorAnimation = foregroundColorAnimation ?? creature.ForegroundColorAnimation
						});
					}
				}
			}
		}

	}

}