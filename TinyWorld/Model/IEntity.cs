using Mathy;

namespace TinyWorld.Model {

	internal interface IEntity {

		Vec2 Vec { get; }

		EntityType Type { get; }
		string GetString { get; }
		int? GetBackgroundColor { get; }

		void AnimationTick ();

		void ActionTick (IMapCommunicator map);

	}

}