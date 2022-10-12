using System;
using UnityEngine;

namespace Player
{
    public class PlayerShooting : IPlayerShooting
    {
        private readonly GameObject _playerObject;
        private readonly Action<Vector2,Vector2,float> _spawnBulletsAction;
    
        private float _shootingDelay = 0.5f;
        private bool _commandedToShoot = false;
        private float _shootingDelayRemaining;

        public PlayerShooting(GameObject playerObject, Action<Vector2,Vector2,float> spawnBulletsAction)
        {
            _playerObject = playerObject;
            _spawnBulletsAction = spawnBulletsAction;
        }

        public void SetShootingDelay(float delay)
        {
            _shootingDelay = delay;
        }

        public void StopShooting()
        {
            _commandedToShoot = false;
        }

        public void StartShooting()
        {
            _commandedToShoot = true;
        }

        public void Update(float time)
        {
            if (_shootingDelayRemaining > 0)
            {
                _shootingDelayRemaining -= time;
            }
            else
            {
                if (_commandedToShoot)
                {
                    Shoot();
                }
            }
        }
    
        private void Shoot()
        {
            _shootingDelayRemaining = _shootingDelay;
            _spawnBulletsAction.Invoke(_playerObject.transform.position,_playerObject.transform.up, _playerObject.transform.eulerAngles.z);
        }
    }
}