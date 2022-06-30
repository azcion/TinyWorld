using Mathy;

namespace TinyWorld.Model {

	internal interface IMapCommunicator {

		Vec2[] GetAvailablePositions (IEntity entity);

		bool PositionIsAvailable (Vec2 vec);

		bool TryMove (IEntity entity, Vec2 vec);

	}

}