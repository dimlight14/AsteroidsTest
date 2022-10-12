using Asteroids;
using UFO;
using UnityEngine;

namespace Managers
{
    public class EnemySpawner
    {
        private readonly IUFOFactory _ufoFactory;
        private readonly IAsteroidsFactory _asteroidsFactory;
        
        private GameObject _playerGameObject;
        
        private const float AsteroidDelay = 10;
        private float _asteroidDelayRemaining = 0;
        private const float UfoDelay = 22;
        private float _ufoDelayRemaining = 0;
        private const int MaxAsteroids = 3;
        private const int MaxUfo = 1;
        private const float SpawnDistance = 10;
        private const float SpawnDistanceMin = 6;

        public EnemySpawner(IUFOFactory ufoFactory,
            IAsteroidsFactory asteroidsFactory)
        {
            _ufoFactory = ufoFactory;
            _asteroidsFactory = asteroidsFactory;
        }

        public void Initialize(GameObject playerObject)
        {
            _playerGameObject = playerObject;
            
            Reset();
        }
        
        public void Reset()
        {
            _asteroidDelayRemaining = 0.5f;
            _ufoDelayRemaining = UfoDelay;
        }
        
        public void Update(float time)
        {
            _asteroidDelayRemaining -= time;
            _ufoDelayRemaining -= time;

            if (_asteroidDelayRemaining <= 0)
            {
                _asteroidDelayRemaining = AsteroidDelay;
                SpawnAsteroid();
            }

            if (_ufoDelayRemaining <= 0)
            {
                _ufoDelayRemaining = UfoDelay;
                SpawnUfo();
            }
        }
        
        private Vector2 GetSpawnPosition()
        {
            var spawnX = Random.Range(-SpawnDistance, SpawnDistance);
            spawnX += spawnX > 0 ? SpawnDistanceMin : -SpawnDistanceMin;
            var spawnY = Random.Range(-SpawnDistance, SpawnDistance);
            spawnY += spawnY > 0 ? SpawnDistanceMin : -SpawnDistanceMin;

            return new Vector2(spawnX, spawnY);
        }

        private void SpawnUfo()
        {
            if (_ufoFactory.ActiveUfo >= MaxUfo) return;

            _ufoFactory.SpawnAt(GetSpawnPosition(), _playerGameObject);
        }

        private void SpawnAsteroid()
        {
            if (_asteroidsFactory.ActiveAsteroids >= MaxAsteroids) return;
            
            _asteroidsFactory.SpawnAsteroidAt(GetSpawnPosition());
        }
    }
}