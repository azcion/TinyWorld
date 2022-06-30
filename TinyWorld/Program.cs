namespace TinyWorld {

	internal static class Program {

		/*               _ _ _         _   _       *
		 *       _____ _| | | |___ ___| |_| |      *
		 *      |_   _|_|___ _|_. |  _| | . |      *
		 *        | | | |   | | |_|_| |_|___|      *
		 *        |_| |_|_|_|_  |                  *
		 *                  |___|                  */

		internal const bool NoSleep =
		#if DEBUG
			true;
		#else
			false;
		#endif

		private static readonly AppConfig.TickConfig TickConfig = new() {
			// Times per second the app updates
			// (input processing, time accumulation)
			TicksPerSecond = 60,

			// Times per second the AnimationTick triggers
			// (ForegroundColorAnimation index advances)
			AnimationTicksPerSecond = 2,

			// Times per second the ActionTick triggers
			// (movement)
			ActionTicksPerSecond = 6
		};

		private static void Main () {
			App app = new(TickConfig);
			app.Run();
		}

	}

}