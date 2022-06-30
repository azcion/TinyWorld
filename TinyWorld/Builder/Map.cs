using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mathy;
using TinyWorld.Model;
using TinyWorld.Util;

namespace TinyWorld.Builder {

	// todo break up class
	internal sealed class Map : ConsoleUtil, IMap, IMapCommunicator {

		internal Map (Vec2 origin, Vec2 size) {
			_origin = origin;
			_size = size;
			_terrainLayer = new IEntity[size.X, size.Y];
			_creatureLayer = new IEntity?[size.X, size.Y];
			_creatures = new List<IEntity>();
		}

		public bool DidChange { get; private set; }

		private readonly Vec2 _origin;
		private readonly Vec2 _size;
		private readonly IEntity[,] _terrainLayer;
		private readonly IEntity?[,] _creatureLayer;
		private readonly List<IEntity> _creatures;

		public Terrain GetTerrainAt (int x, int y) {
			return (Terrain)_terrainLayer[x, y];
		}

		public void Place (IEntity entity) {
			switch (entity.Type) {
				case EntityType.Terrain:
					_terrainLayer[entity.Vec.X, entity.Vec.Y] = entity;

					break;
				case EntityType.Creature:
					_creatureLayer[entity.Vec.X, entity.Vec.Y] = entity;
					_creatures.Add(entity);

					break;
			}

			DidChange = true;
		}

		public Vec2[] GetAvailablePositions (IEntity entity) {
			List<Vec2> result = new();
			int x = entity.Vec.X;
			int y = entity.Vec.Y;
			Biome[]? traversalBiomes = ((Creature)entity).TraversalBiomes;

			if (traversalBiomes == null) {
				return result.ToArray();
			}

			if (x > 0 && _creatureLayer[x - 1, y] == null
					&& traversalBiomes.Contains(((Terrain)_terrainLayer[x - 1, y]).ThisBiome)) {
				result.Add(new Vec2(x - 1, y));
			}

			if (x < _size.X - 1 && _creatureLayer[x + 1, y] == null
					&& traversalBiomes.Contains(((Terrain)_terrainLayer[x + 1, y]).ThisBiome)) {
				result.Add(new Vec2(x + 1, y));
			}

			if (y > 0 && _creatureLayer[x, y - 1] == null
					&& traversalBiomes.Contains(((Terrain)_terrainLayer[x, y - 1]).ThisBiome)) {
				result.Add(new Vec2(x, y - 1));
			}

			if (y < _size.Y - 1 && _creatureLayer[x, y + 1] == null
					&& traversalBiomes.Contains(((Terrain)_terrainLayer[x, y + 1]).ThisBiome)) {
				result.Add(new Vec2(x, y + 1));
			}

			return result.ToArray();
		}

		public bool PositionIsAvailable (Vec2 vec) {
			return _creatureLayer[vec.X, vec.Y] == null;
		}

		public bool TryMove (IEntity entity, Vec2 vec) {
			if (_creatureLayer[vec.X, vec.Y] != null) {
				return false;
			}

			_creatureLayer[vec.X, vec.Y] = entity;
			_creatureLayer[entity.Vec.X, entity.Vec.Y] = null;

			// Overwrite old position
			Position(_origin + entity.Vec);
			Write(_terrainLayer[entity.Vec.X, entity.Vec.Y].GetString);

			// Overwrite new position
			Position(_origin + vec);
			Write($"\x1b[48;5;{_terrainLayer[vec.X, vec.Y].GetBackgroundColor}m{entity.GetString}");

			return true;
		}

		public void AnimationTick () {
			_creatures.ForEach(creature => creature.AnimationTick());
		}

		public void ActionTick () {
			_creatures.ForEach(creature => creature.ActionTick(this));
		}

		public void DrawCreatures () {
			_creatures.ForEach(creature => {
				Position(_origin + creature.Vec);

				if (creature.GetBackgroundColor == null) {
					int color = _terrainLayer[creature.Vec.X, creature.Vec.Y]
						.GetBackgroundColor ?? 201;

					Write($"\x1b[48;5;{color}m{creature.GetString}");
				} else {
					Write(creature.GetString);
				}
			});
		}

		public void DrawMap () {
			DidChange = false;
			IEntity[,] flattenedMap = GetFlattenedMap();
			StringBuilder line = new();

			for (int y = 0; y < _size.Y; y++) {
				for (int x = 0; x < _size.X; x++) {
					line.Append(flattenedMap[x, y].GetString);
				}

				Position(_origin.X, _origin.Y + y);
				Write(line.ToString());
				line.Clear();
			}
		}

		private IEntity[,] GetFlattenedMap () {
			IEntity[,] result = new IEntity[_size.X, _size.Y];

			for (int y = 0; y < _size.Y; y++) {
				for (int x = 0; x < _size.X; x++) {
					result[x, y] = _creatureLayer[x, y] ?? _terrainLayer[x, y];
				}
			}

			return result;
		}

	}

}