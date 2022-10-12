using System;
using System.Collections.Generic;
using Main;
using Physics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Bullets
{
    public class BulletsFactory : IBulletsFactory
    {
        private const int EnemySpeed = 10;
        
        private readonly GameObject _bulletReference;
        private readonly Vector2 _screenSize;
        private readonly Updater _updater;
        private readonly ICollisionDetectorBullets _colliderDetection;
        private readonly GameObject _laserReference;
        private readonly GameObject _enemyBulletReference;

        private readonly Queue<BulletController> _bulletsPool = new Queue<BulletController>();
        private readonly List<BulletController> _activeBullets = new List<BulletController>();
        private readonly Queue<BulletController> _lasersPool = new Queue<BulletController>();
        private readonly List<BulletController> _activeLasers = new List<BulletController>();
        private readonly Queue<BulletController> _enemyBulletsPool = new Queue<BulletController>();
        private readonly List<BulletController> _activeEnemyBullets = new List<BulletController>();

        private readonly Dictionary<BulletController, BulletCollider> _colliders =
            new Dictionary<BulletController, BulletCollider>();

        public BulletsFactory(Vector2 screenSize, GameObject bulletReference, GameObject laserReference,
            GameObject enemyBulletReference, Updater updater,
            ICollisionDetectorBullets colliderDetection)
        {
            _bulletReference = bulletReference;
            _screenSize = screenSize;
            _enemyBulletReference = enemyBulletReference;
            _updater = updater;
            _colliderDetection = colliderDetection;
            _laserReference = laserReference;
        }

        public void SpawnBullet(Vector2 position, Vector2 direction, float rotation)
        {
            var newBullet = _bulletsPool.Count == 0
                ? CreateNewBullet(position, _bulletReference, Despawn)
                : _bulletsPool.Dequeue();
            _activeBullets.Add(newBullet);
            newBullet.GetFired(direction, position, rotation);
            _colliderDetection.RegisterBullet(_colliders[newBullet]);
        }

        private BulletController CreateNewBullet(Vector2 position, GameObject reference,
            Action<BulletController> despawnAction)
        {
            var gameObject = Object.Instantiate(reference, position, Quaternion.identity);
            var viewObject = gameObject.transform.GetChild(0);
            var boxCollider = gameObject.GetComponent<BoxCollider2D>();
            var movement = new BaseMovement(_screenSize, gameObject);
            var bulletScript = new BulletController(gameObject, _updater, viewObject, movement, despawnAction);
            var colliderScript = new BulletCollider(boxCollider, bulletScript.OnCollision);
            _colliders.Add(bulletScript, colliderScript);
            return bulletScript;
        }
        
        public void SpawnLaser(Vector2 position, Vector2 direction, float rotation)
        {
            var newBullet = _lasersPool.Count == 0
                ? CreateNewBullet(position, _laserReference, Despawn)
                : _lasersPool.Dequeue();
            _activeLasers.Add(newBullet);
            newBullet.GetFired(direction, position, rotation);
            newBullet.MarkAsLaser();
            _colliders[newBullet].MarkAsLaser();
            _colliderDetection.RegisterBullet(_colliders[newBullet]);
        }

        public void SpawnEnemyBullet(Vector2 position, Vector2 direction, float rotation)
        {
            var newBullet = _enemyBulletsPool.Count == 0
                ? CreateNewBullet(position, _enemyBulletReference, DespawnEnemy)
                : _enemyBulletsPool.Dequeue();
            _activeEnemyBullets.Add(newBullet);
            newBullet.GetFired(direction, position, rotation);
            newBullet.SetSpeed(EnemySpeed);
            _colliderDetection.RegisterEnemyBullet(_colliders[newBullet]);
        }

        public void ClearAll()
        {
            var activeCopy = new BulletController[_activeBullets.Count];
            _activeBullets.CopyTo(activeCopy);
            foreach (var bullet in activeCopy)
            {
                Despawn(bullet);
            }
            
            activeCopy = new BulletController[_activeLasers.Count];
            _activeLasers.CopyTo(activeCopy);
            foreach (var bullet in activeCopy)
            {
                Despawn(bullet);
            }
            
            activeCopy = new BulletController[_activeEnemyBullets.Count];
            _activeEnemyBullets.CopyTo(activeCopy);
            foreach (var bullet in activeCopy)
            {
                DespawnEnemy(bullet);
            }
        }

        private void Despawn(BulletController bulletScript)
        {
            if (_activeLasers.Contains(bulletScript))
            {
                _activeLasers.Remove(bulletScript);
                _lasersPool.Enqueue(bulletScript);
            }
            else
            {
                _activeBullets.Remove(bulletScript);
                _bulletsPool.Enqueue(bulletScript);
            }

            _colliderDetection.UnregisterBullet(_colliders[bulletScript]);
            bulletScript.HideAndDisable();
        }

        private void DespawnEnemy(BulletController bulletScript)
        {
            _activeEnemyBullets.Remove(bulletScript);
            _enemyBulletsPool.Enqueue(bulletScript);

            _colliderDetection.UnregisterBullet(_colliders[bulletScript]);
            bulletScript.HideAndDisable();
        }
    }
}