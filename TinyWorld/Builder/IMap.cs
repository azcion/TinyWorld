using TinyWorld.Model;

namespace TinyWorld.Builder {

	internal interface IMap {

		bool DidChange { get; }

		void DrawMap ();

		void ActionTick ();

		void AnimationTick ();

		void DrawCreatures ();

		void Place (IEntity entity);

		Terrain GetTerrainAt (int x, int y);

	}

}