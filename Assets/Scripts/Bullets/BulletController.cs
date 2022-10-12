using System;
using Main;
using Physics;
using UnityEngine;

namespace Bullets
{
    public class BulletController
    {
        private const float Lifetime = 1.8f;
        private const float LaserSpeed = 18;
        
        private readonly IBaseMovement _movement;
        private readonly GameObject _bulletObject;
        private readonly Updater _updater;
        private readonly Action<BulletController> _despawnAction;
        private readonly Transform _viewObject;
        private readonly float _speed = 14f;
        
        private float _lifetimeRemaining;

        public BulletController(
            GameObject bulletObject,
            Updater updater,
            Transform viewObject,
            IBaseMovement movement,
            Action<BulletController> despawnAction)
        {
            _bulletObject = bulletObject;
            _movement = movement;
            _movement.SetSpeed(_speed);
            _updater = updater;
            _despawnAction = despawnAction;
            _viewObject = viewObject;
        }
        public void GetFired(Vector2 direction, Vector2 position, float rotation)
        {
            _updater.OnUpdate += Update;
            _updater.OnFixedUpdate += FixedUpdate;

            _bulletObject.transform.position = position;
            var eulers = _viewObject.eulerAngles;
            _viewObject.eulerAngles = new Vector3(eulers.x, eulers.y, rotation);
            _movement.SetDirection(direction.normalized);
            _lifetimeRemaining = Lifetime;
            _bulletObject.SetActive(true);
        }

        private void Update(float time)
        {
            _lifetimeRemaining -= time;
            if (_lifetimeRemaining <= 0)
            {
                Despawn();
            }
        }

        private void FixedUpdate(float time)
        {
            _movement.MoveForward(time);
        }

        private void Despawn()
        {
            _despawnAction.Invoke(this);
        }

        public void OnCollision()
        {
            Despawn();
        }

        public void MarkAsLaser()
        {
            _movement.SetSpeed(LaserSpeed);
        }

        public void SetSpeed(int speed)
        {
            _movement.SetSpeed(speed);
        }

        public void HideAndDisable()
        {
            _updater.OnUpdate -= Update;
            _updater.OnFixedUpdate -= FixedUpdate;
            _bulletObject.SetActive(false);
        }
    }
}