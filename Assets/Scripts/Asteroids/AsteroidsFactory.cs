using System.Collections.Generic;
using Main;
using Physics;
using UI;
using UnityEngine;

namespace Asteroids
{
    public class AsteroidsFactory : IAsteroidsFactory, IAsteroidsFactoryService
    {
        private const float NormalAsteroidSpeed = 3;
        private const float SmallAsteroidSpeed = 6;
        private const int Bounty = 10;

        private readonly GameObject _asteroidPrefabReference;
        private readonly GameObject _smallAsteroidReference;
        private readonly ICollisionDetectorEnemy _collisionDetector;
        private readonly Updater _updater;
        private readonly Vector2 _screenSize;
        private readonly ScoresController _scoresController;

        private readonly Queue<AsteroidController> _asteroidsPool = new Queue<AsteroidController>();
        private readonly List<AsteroidController> _activeAsteroids = new List<AsteroidController>();
        private readonly Queue<AsteroidController> _smallAsteroidsPool = new Queue<AsteroidController>();
        private readonly List<AsteroidController> _smallActiveAsteroids = new List<AsteroidController>();

        private readonly Dictionary<AsteroidController, EnemyCollider> _colliders =
            new Dictionary<AsteroidController, EnemyCollider>();

        public int ActiveAsteroids => _activeAsteroids.Count;

        public AsteroidsFactory(
            GameObject asteroidPrefabReference,
            GameObject smallAsteroidReference,
            ICollisionDetectorEnemy collisionDetector,
            Updater updater,
            Vector2 screenSize,
            ScoresController scoresController)
        {
            _asteroidPrefabReference = asteroidPrefabReference;
            _smallAsteroidReference = smallAsteroidReference;
            _collisionDetector = collisionDetector;
            _updater = updater;
            _screenSize = screenSize;
            _scoresController = scoresController;
        }

        public void ClearAll()
        {
            var activeCopy = new AsteroidController[_activeAsteroids.Count];
            _activeAsteroids.CopyTo(activeCopy);
            foreach (var asteroid in activeCopy)
            {
                DespawnAsteroid(asteroid);
            }

            activeCopy = new AsteroidController[_smallActiveAsteroids.Count];
            _smallActiveAsteroids.CopyTo(activeCopy);
            foreach (var asteroid in activeCopy)
            {
                DespawnAsteroid(asteroid);
            }
        }

        public void SpawnAsteroidAt(Vector2 position)
        {
            var newAsteroid = _asteroidsPool.Count > 0 ? _asteroidsPool.Dequeue() : CreateNewAsteroid(position);
            _activeAsteroids.Add(newAsteroid);
            newAsteroid.Initialize(position);
            _collisionDetector.RegisterEnemy(_colliders[newAsteroid]);
            _colliders[newAsteroid].SetActive(true);
        }
        public void SpawnSmallAsteroidsAroundPoint(Vector2 position)
        {
            var randomisedPosition = new Vector2(position.x - 0.8f, position.y + 0.8f);
            SpawnSmallAsteroidAt(randomisedPosition);
            randomisedPosition = new Vector2(position.x + 0.8f, position.y + 0.8f);
            SpawnSmallAsteroidAt(randomisedPosition);
            randomisedPosition = new Vector2(position.x, position.y - 0.8f);
            SpawnSmallAsteroidAt(randomisedPosition);
        }

        private void SpawnSmallAsteroidAt(Vector2 position)
        {
            var newAsteroid = _smallAsteroidsPool.Count > 0
                ? _smallAsteroidsPool.Dequeue()
                : CreateNewSmallAsteroid(position);
            _smallActiveAsteroids.Add(newAsteroid);
            newAsteroid.Initialize(position);
            _collisionDetector.RegisterEnemy(_colliders[newAsteroid]);
            _colliders[newAsteroid].SetActive(true);
        }

        private AsteroidController CreateNewSmallAsteroid(Vector2 position)
        {
            var newAsteroidObject = Object.Instantiate(_smallAsteroidReference, position, Quaternion.identity);
            var movement = new BaseMovement(_screenSize, newAsteroidObject);
            var collider = newAsteroidObject.GetComponent<BoxCollider2D>();
            var bounty = new BountyComponent(Bounty, _scoresController.ClaimBounty);
            var asteroidScript =
                new SmallAsteroidsController(_updater, newAsteroidObject, movement, this, SmallAsteroidSpeed, bounty);
            var colliderScript = new EnemyCollider(collider, asteroidScript.GetWrecked);
            _colliders.Add(asteroidScript, colliderScript);
            return asteroidScript;
        }

        private AsteroidController CreateNewAsteroid(Vector2 position)
        {
            var newAsteroidObject = Object.Instantiate(_asteroidPrefabReference, position, Quaternion.identity);
            var movement = new BaseMovement(_screenSize, newAsteroidObject);
            var collider = newAsteroidObject.GetComponent<BoxCollider2D>();
            var bounty = new BountyComponent(Bounty, _scoresController.ClaimBounty);
            var asteroidScript =
                new AsteroidController(_updater, newAsteroidObject, movement, this, NormalAsteroidSpeed, bounty);
            var colliderScript = new EnemyCollider(collider, asteroidScript.GetWrecked);
            _colliders.Add(asteroidScript, colliderScript);
            return asteroidScript;
        }

        public void DespawnAsteroid(AsteroidController asteroid)
        {
            if (_activeAsteroids.Contains(asteroid))
            {
                _activeAsteroids.Remove(asteroid);
                _asteroidsPool.Enqueue(asteroid);
            }
            else
            {
                _smallActiveAsteroids.Remove(asteroid);
                _smallAsteroidsPool.Enqueue(asteroid);
            }

            _colliders[asteroid].SetActive(false);
            asteroid.HideAndDisable();
        }
    }
}