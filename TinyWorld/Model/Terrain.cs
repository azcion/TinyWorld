using System.Text;
using Mathy;

namespace TinyWorld.Model {

	internal record Terrain : Entity, IEntity {

		public Vec2 Vec { get; init; }

		public string GetString {
			get { return _cachedString ??= BuildString(); }
		}

		public int? GetBackgroundColor => BackgroundColor;
		public EntityType Type => EntityType.Terrain;

		private bool FlatSymbolRatioBool => Rand.NextDouble() <= FlatSymbolRatio;

		private string? _cachedString;

		public void AnimationTick () {
			_cachedString = BuildString();
		}

		public void ActionTick (IMapCommunicator map) { }

		private string BuildString () {
			StringBuilder result = new();

			if (UseBackgroundColor) {
				result.Append($"\x1b[48;5;{BackgroundColor}m");
			}

			if (UseForegroundColor) {
				result.Append($"\x1b[38;5;{ForegroundColor}m");
			} else if (UseForegroundColorArray) {
				result.Append($"\x1b[38;5;{GetRandom(ForegroundColorArray!)}m");
			}

			if (UseSymbol) {
				result.Append(Symbol);
			} else if (UseRandomSymbolArray) {
				if (!UseFlatSymbol || !UseFlatSymbolRatio || !FlatSymbolRatioBool) {
					result.Append(GetRandom(RandomSymbolArray!));
				} else {
					result.Append(FlatSymbol);
				}
			}

			return result.ToString();
		}

	}

}