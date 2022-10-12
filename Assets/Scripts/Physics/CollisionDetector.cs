using System.Collections.Generic;
using Main;

namespace Physics
{
    public class CollisionDetector : ICollisionDetectorPlayer, ICollisionDetectorBullets, ICollisionDetectorEnemy, ICollisionDetectorInitializable
    {
        private readonly Updater _updater;
        
        private ISimpleCollider _player;
        private readonly List<IBulletCollider> _playerBullets = new List<IBulletCollider>();
        private readonly List<IBulletCollider> _enemyBullets = new List<IBulletCollider>();
        private readonly List<IEnemyCollider> _enemies = new List<IEnemyCollider>();

        private readonly List<IEnemyCollider> _enemiesToRemove = new List<IEnemyCollider>();
        private readonly List<IBulletCollider> _bulletsToRemove = new List<IBulletCollider>();

        public CollisionDetector(Updater updater)
        {
            _updater = updater;
        }

        public void Initialize()
        {
            _updater.OnFixedUpdate += _ => CheckCollisions();
        }

        public void ClearAll()
        {
            CleanCollections();
            _playerBullets.Clear();
            _enemyBullets.Clear();
            _enemies.Clear();
        }

        public void RegisterPlayer(ISimpleCollider playerController)
        {
            _player = playerController;
        }

        public void RegisterBullet(IBulletCollider bulletScript)
        {
            _playerBullets.Add(bulletScript);
        }
        
        public void RegisterEnemyBullet(IBulletCollider bulletScript)
        {
            _enemyBullets.Add(bulletScript);
        }

        public void RegisterEnemy(IEnemyCollider enemy)
        {
            _enemies.Add(enemy);
        }
        
        public void UnregisterBullet(IBulletCollider bulletScript)
        {
            if (!_bulletsToRemove.Contains(bulletScript))
            {
                _bulletsToRemove.Add(bulletScript);
            }
        }

        private void CheckCollisions()
        {
            CleanCollections();
            CheckBulletsCollisions();
            CheckPlayerCollisions();
        }

        private void CheckBulletsCollisions()
        {
            foreach (var bulletScript in _playerBullets)
            {
                var collider = bulletScript.GetCollider().bounds;
                foreach (var enemy in _enemies)
                {
                    if(!enemy.IsActive) continue;
                    
                    var secondCollider = enemy.GetCollider().bounds;
                    var hasHit = collider.Intersects(secondCollider);
                    if (hasHit)
                    {
                        enemy.GetWrecked(bulletScript.IsLaser);
                        if(!bulletScript.IsLaser)bulletScript.OnCollision();
                        _enemiesToRemove.Add(enemy);
                        break;
                    }
                }
            }
        }

        private void CheckPlayerCollisions()
        {
            var playerCollider = _player.GetCollider().bounds;
            foreach (var enemy in _enemies)
            {
                var collider = enemy.GetCollider().bounds;
                var hasHit = collider.Intersects(playerCollider);
                if (hasHit)
                {
                    enemy.GetWrecked(false);
                    _enemiesToRemove.Add(enemy);
                    _player.OnCollision();
                    break;
                }
            }
            foreach (var bullet in _enemyBullets)
            {
                var collider = bullet.GetCollider().bounds;
                var hasHit = collider.Intersects(playerCollider);
                if (hasHit)
                {
                    bullet.OnCollision();
                    _player.OnCollision();
                    break;
                }
            }
        }

        private void CleanCollections()
        {
            foreach (var enemy in _enemiesToRemove)
            {
                _enemies.Remove(enemy);
            }

            foreach (var bullet in _bulletsToRemove)
            {
                if (_playerBullets.Contains(bullet))
                {
                    _playerBullets.Remove(bullet);
                }else if (_enemyBullets.Contains(bullet))
                {
                    _enemyBullets.Remove(bullet);
                }
            }
            _enemiesToRemove.Clear();
            _bulletsToRemove.Clear();
        }
    }
}