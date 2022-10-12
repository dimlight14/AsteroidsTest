using Main;
using Physics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids
{
    public class AsteroidController
    {
        private readonly float _speed = 3;
        private readonly Updater _updater;
        private readonly GameObject _gameObject;
        private readonly IBaseMovement _asteroidMovement;
        private readonly IAsteroidsFactoryService _factory;
        protected readonly IBountyComponent _bountyComponent;
        public AsteroidController(Updater updater,
            GameObject gameObject,
            IBaseMovement baseMovement,
            IAsteroidsFactoryService factory,
            float speed, 
            IBountyComponent bountyComponent)
        {
            _updater = updater;
            _gameObject = gameObject;
            _factory = factory;
            _asteroidMovement = baseMovement;
            _asteroidMovement.SetSpeed(_speed);
            _speed = speed;
            _bountyComponent = bountyComponent;
        }

        public void Initialize(Vector2 position)
        {
            _gameObject.SetActive(true);
            _gameObject.transform.position = position;
            var direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            _asteroidMovement.SetDirection(direction.normalized);
            _updater.OnFixedUpdate += Update;
        }

        private void Update(float time)
        {
            _asteroidMovement.MoveForward(time);
        }

        public virtual void GetWrecked(bool laserHit)
        {
            if (!laserHit)
            {
                _factory.SpawnSmallAsteroidsAroundPoint(_gameObject.transform.position);
            }
            
            _bountyComponent.Claim();
            Despawn();
        }

        public void HideAndDisable()
        {
            _updater.OnFixedUpdate -= Update;
            _gameObject.SetActive(false);
        }

        protected void Despawn()
        {
            _factory.DespawnAsteroid(this);
        }
    }
}