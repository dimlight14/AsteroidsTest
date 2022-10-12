using Bullets;
using Main;
using Physics;
using UnityEngine;

namespace Player
{
    public class PlayerInfoObject
    {
        public PlayerController Controller;
        public GameObject GameObject;
        public IPlayerSpeedProvider Movement;
        public IPlayerLaserInfoProvider LaserShooter;
    }

    public class PlayerFactory : IPlayerFactory
    {
        private readonly Updater _updater;
        private readonly Vector2 _screenSize;
        private readonly IBulletsFactory _bulletsFactory;
        private readonly GameObject _playerReference;
        private readonly ICollisionDetectorPlayer _collisionDetector;
        
        private ISimpleCollider _playerCollider;

        public PlayerFactory(Updater updater,
            Vector2 screenSize,
            IBulletsFactory bulletsFactory,
            GameObject playerReference,
            ICollisionDetectorPlayer collisionDetector
        )
        {
            _updater = updater;
            _screenSize = screenSize;
            _bulletsFactory = bulletsFactory;
            _playerReference = playerReference;
            _collisionDetector = collisionDetector;
        }

        public PlayerInfoObject CreatePlayer()
        {
            var playerObject = Object.Instantiate(_playerReference, Vector3.zero, Quaternion.identity);
            var shooting = new PlayerShooting(playerObject, _bulletsFactory.SpawnBullet);
            var playerMovement = new PlayerMovement(playerObject, _screenSize);
            var playerRotation = new PlayerRotation(playerObject);
            var playerLaser = new PlayerLaserShooter(_bulletsFactory.SpawnLaser, playerObject);
            var hitBox = playerObject.GetComponent<BoxCollider2D>();

            var newPlayer = new PlayerController(_updater, shooting, playerMovement, playerRotation, playerLaser,
                playerObject);
            _playerCollider = new SimpleCollider(hitBox, newPlayer.GetDamaged);
            _collisionDetector.RegisterPlayer(_playerCollider);

            var infoContainer = new PlayerInfoObject()
            {
                Controller =  newPlayer,
                GameObject = playerObject,
                Movement = playerMovement,
                LaserShooter = playerLaser
            };
            
            return infoContainer;
        }
    }
}