using System;
using UnityEngine;

namespace Player
{
    public class PlayerLaserShooter : IPlayerLaserShooter, IPlayerLaserInfoProvider
    {
        private const float Cooldown = 4;
        private const int MaxCharges = 4;
        
        private readonly Action<Vector2,Vector2,float> _spawnBulletsAction;
        private readonly GameObject _playerObject;

        private float _cooldownRemaining;
        private int _numberOfCharges = 1;

        private bool CanShoot => _numberOfCharges > 0;

        public PlayerLaserShooter(Action<Vector2,Vector2,float> spawnBulletsAction, GameObject playerObject)
        {
            _spawnBulletsAction = spawnBulletsAction;
            _playerObject = playerObject;
        }

        public void Update(float time)
        {
            if (_numberOfCharges < MaxCharges)
            {
                _cooldownRemaining -= time;
                if (_cooldownRemaining <= 0)
                {
                    _cooldownRemaining = Cooldown;
                    _numberOfCharges++;
                }
            }
        }

        public void TryShooting()
        {
            if (!CanShoot) return;

            _spawnBulletsAction.Invoke(_playerObject.transform.position, _playerObject.transform.up, _playerObject.transform.eulerAngles.z);
            _numberOfCharges--;
        }

        public int GetShotsRemaining()
        {
            return _numberOfCharges;
        }

        public float GetCooldownRemaining()
        {
            return _cooldownRemaining / Cooldown;
        }
    }
}