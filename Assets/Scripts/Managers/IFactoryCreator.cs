using Asteroids;
using Bullets;
using Physics;
using Player;
using UFO;
using UI;

namespace Managers
{
    public interface IFactoryCreator
    {
        ICollisionDetectorInitializable GetCollisionDetector();
        IBulletsFactory GetBulletFactory();
        IUFOFactory GetUfoFactory(IBulletsFactory bulletsFactory, ScoresController scoresController);
        IAsteroidsFactory GetAsteroidsFactory(ScoresController scoresController);
        IPlayerFactory GetPlayerFactory(IBulletsFactory bulletsFactory);
    }
}