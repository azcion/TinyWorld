using System.Text;
using Mathy;

namespace TinyWorld.Model {

	internal record Creature : Entity, IEntity {

		public Vec2 Vec { get; set; }

		public string GetString {
			get { return _cachedString ??= BuildString(false); }
		}

		public int? GetBackgroundColor => BackgroundColor;
		public EntityType Type => EntityType.Creature;

		private string? _cachedString;
		private int _actionFrame;
		private int _backgroundColorAnimationIndex;
		private int _foregroundColorAnimationIndex;
		private int _symbolAnimationIndex;

		public void AnimationTick () {
			_cachedString = BuildString(true);
		}

		public void ActionTick (IMapCommunicator map) {
			if (!CanMove) {
				return;
			}

			if (UseRunNthActionFrame) {
				if (_actionFrame != RunNthActionFrame) {
					++_actionFrame;

					return;
				}

				_actionFrame = 0;
			}

			Vec2[] positions = map.GetAvailablePositions(this);

			if (positions.Length == 0) {
				// Can't move due to no available position
				return;
			}

			Vec2 vec = GetRandom(positions);

			if (map.TryMove(this, vec)) {
				Vec = vec;
				BuildString(false);
			}
		}

		private string BuildString (bool advanceAnimation) {
			StringBuilder result = new();

			if (UseBackgroundColor) {
				result.Append($"\x1b[48;5;{BackgroundColor}m");
			} else if (UseBackgroundColorAnimation) {
				int color = advanceAnimation
					? GetNext(BackgroundColorAnimation!, ref _backgroundColorAnimationIndex)
					: BackgroundColorAnimation![_backgroundColorAnimationIndex];
				result.Append($"\x1b[48;5;{color}m");
			}

			if (UseForegroundColor) {
				result.Append($"\x1b[38;5;{ForegroundColor}m");
			} else if (UseForegroundColorArray) {
				result.Append($"\x1b[38;5;{GetRandom(ForegroundColorArray!)}m");
			} else if (UseForegroundColorAnimation) {
				int color = advanceAnimation
					? GetNext(ForegroundColorAnimation!, ref _foregroundColorAnimationIndex)
					: ForegroundColorAnimation![_foregroundColorAnimationIndex];
				result.Append($"\x1b[38;5;{color}m");
			}

			if (UseSymbol) {
				result.Append(Symbol);
			} else if (UseSymbolAnimation) {
				char symbol = advanceAnimation
					? GetNext(SymbolAnimation!, ref _symbolAnimationIndex)
					: SymbolAnimation![_symbolAnimationIndex];
				result.Append(symbol);
			}

			return result.ToString();
		}

	}

}