using TinyWorld.Util;

namespace TinyWorld {

	internal class AppConfig : ConsoleAdjustments {

		internal sealed record TickConfig {

			internal uint TicksPerSecond { get; init; }
			internal uint AnimationTicksPerSecond { get; init; }
			internal uint ActionTicksPerSecond { get; init; }

		}

		protected TickConfig Ticks = null!;
		protected double SecondsPerTick { get; private set; }
		protected double SecondsPerAnimationTick { get; private set; }
		protected double SecondsPerActionTick { get; private set; }

		protected void ConvertTicks () {
			SecondsPerTick = Ticks.TicksPerSecond == 0
				? double.MaxValue
				: 1d / Ticks.TicksPerSecond;

			SecondsPerAnimationTick = Ticks.AnimationTicksPerSecond == 0
				? double.MaxValue
				: 1d / Ticks.AnimationTicksPerSecond;

			SecondsPerActionTick = Ticks.ActionTicksPerSecond == 0
				? double.MaxValue
				: 1d / Ticks.ActionTicksPerSecond;
		}

	}

}