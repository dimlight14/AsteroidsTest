using Asteroids;
using Bullets;
using Main;
using Physics;
using Player;
using UFO;
using UI;
using UnityEngine;

namespace Managers
{
    public class FactoryCreator : IFactoryCreator
    {
        private readonly PrefabsMap _prefabsMap;
        private readonly Updater _updater;
        private readonly Vector2 _screenSize;
        private readonly CollisionDetector _collisionDetector;

        public FactoryCreator(PrefabsMap prefabsMap,
            Camera mainCamera,
            Updater updater)
        {
            _prefabsMap = prefabsMap;
            _updater = updater;
            
            var orthSize = mainCamera.orthographicSize * 2;
            _screenSize = new Vector2(orthSize * mainCamera.aspect, orthSize);

            _collisionDetector = new CollisionDetector(updater);
        }
        
        public ICollisionDetectorInitializable GetCollisionDetector()
        {
            return _collisionDetector;
        }

        public IBulletsFactory GetBulletFactory()
        {
            return  new BulletsFactory(_screenSize, _prefabsMap.BulletReference, _prefabsMap.LaserReference,
                _prefabsMap.EnemyBulletReference, _updater, _collisionDetector);
        }

        public IUFOFactory GetUfoFactory(IBulletsFactory bulletsFactory, ScoresController scoresController)
        {
            return new UFOFactory(_prefabsMap.UfoReference, _updater, _collisionDetector, bulletsFactory, _screenSize,scoresController);
        }

        public IAsteroidsFactory GetAsteroidsFactory(ScoresController scoresController)
        {
            return new AsteroidsFactory(_prefabsMap.AsteroidObjectReference, _prefabsMap.AsteroidSmallObjectReference,
                _collisionDetector, _updater, _screenSize, scoresController);
        }

        public IPlayerFactory GetPlayerFactory(IBulletsFactory bulletsFactory)
        {
            return new PlayerFactory(_updater, _screenSize, bulletsFactory, _prefabsMap.PlayerObjectReference, _collisionDetector);
        }
    }
}