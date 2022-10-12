using System.Collections.Generic;
using Bullets;
using Main;
using Physics;
using Player;
using UI;
using UnityEngine;

namespace UFO
{
    public class UFOFactory : IUFOFactory
    {
        private const float MaxSpeed = 4;
        private const float Acceleration = 7;
        private const float ShootingDelay = 5f;
        private const int Bounty = 30;

        private readonly GameObject _objectReference;
        private readonly Updater _updater;
        private readonly ICollisionDetectorEnemy _collisionDetector;
        private readonly IBulletsFactory _bulletsFactory;
        private readonly Vector2 _screenSize;
        private readonly ScoresController _scoresController;

        private readonly Queue<UFOController> _ufoPool = new Queue<UFOController>();
        private readonly List<UFOController> _activeUfo = new List<UFOController>();

        private readonly Dictionary<UFOController, EnemyCollider> _ufoColliders =
            new Dictionary<UFOController, EnemyCollider>();

        private GameObject _playerObject;
        public int ActiveUfo => _activeUfo.Count;

        public UFOFactory(
            GameObject objectReference,
            Updater updater,
            ICollisionDetectorEnemy collisionDetector,
            IBulletsFactory bulletsFactory,
            Vector2 screenSize,
            ScoresController scoresController)
        {
            _objectReference = objectReference;
            _updater = updater;
            _collisionDetector = collisionDetector;
            _bulletsFactory = bulletsFactory;
            _screenSize = screenSize;
            _scoresController = scoresController;
        }


        public void SpawnAt(Vector2 position, GameObject playerController)
        {
            _playerObject = playerController;
            var newUfo = _ufoPool.Count > 0 ? _ufoPool.Dequeue() : CreateUfo(position);
            _activeUfo.Add(newUfo);
            newUfo.Initialize();
            _collisionDetector.RegisterEnemy(_ufoColliders[newUfo]);
            _ufoColliders[newUfo].SetActive(true);
        }

        public void ClearAll()
        {
            var activeCopy = new UFOController[_activeUfo.Count];
            _activeUfo.CopyTo(activeCopy);
            foreach (var ufo in activeCopy)
            {
                Despawn(ufo);
            }
        }

        private UFOController CreateUfo(Vector2 position)
        {
            var newUfoObject = Object.Instantiate(_objectReference, position, Quaternion.identity);
            var shooting = new PlayerShooting(newUfoObject, _bulletsFactory.SpawnEnemyBullet);
            shooting.SetShootingDelay(ShootingDelay);
            var movement = new PlayerMovement(newUfoObject, _screenSize);
            movement.SetMaxSpeedAndAcceleration(MaxSpeed, Acceleration);
            var rotation = new PlayerRotation(newUfoObject);
            var collider = newUfoObject.GetComponent<BoxCollider2D>();
            var bounty = new BountyComponent(Bounty, _scoresController.ClaimBounty);
            
            var ufoScript = new UFOController(newUfoObject, _playerObject, _updater, Despawn, shooting,
                movement, rotation, bounty);
            var colliderScript = new EnemyCollider(collider, ufoScript.GetWrecked);
            _ufoColliders.Add(ufoScript, colliderScript);

            return ufoScript;
        }

        private void Despawn(UFOController ufoScript)
        {
            _ufoPool.Enqueue(ufoScript);
            _activeUfo.Remove(ufoScript);
            _ufoColliders[ufoScript].SetActive(false);
            ufoScript.HideAndDisable();
        }
    }
}