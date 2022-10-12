using System;
using Main;
using UnityEngine;

namespace Player
{
    public class PlayerController : IPlayerPositionProvider
    {
        private readonly IPlayerShooting _playerShooting;
        private readonly IPlayerMovement _playerMovement;
        private readonly IPlayerRotation _playerRotation;
        private readonly IPlayerLaserShooter _laserShooter;
        private readonly Updater _updater;
        private readonly GameObject _gameObject;
    
        private InputMap _inputMap;
        public Action OnPlayerDestroyed;

        public PlayerController(
            Updater updater,
            IPlayerShooting playerShooting,
            IPlayerMovement playerMovement,
            IPlayerRotation playerRotation,
            IPlayerLaserShooter laserShooter,
            GameObject gameObject
        )
        {
            _updater = updater;
            _playerShooting = playerShooting;
            _playerMovement = playerMovement;
            _playerRotation = playerRotation;
            _laserShooter = laserShooter;
            _gameObject = gameObject;
        }

        public void Initialize()
        {
            _updater.OnUpdate += Update;
            _updater.OnFixedUpdate += FixedUpdate;
        
            _inputMap = new InputMap();
            _inputMap.Player.Shoot.Enable();
            _inputMap.Player.Shoot.performed += _ => _playerShooting.StartShooting();
            _inputMap.Player.Shoot.canceled += _ => _playerShooting.StopShooting();

            _inputMap.Player.ShootLaser.Enable();
            _inputMap.Player.ShootLaser.performed += _ => _laserShooter.TryShooting();

            _inputMap.Player.Movement.Enable();
        }
    
        public void GetDamaged()
        {
            OnPlayerDestroyed?.Invoke();
        }

        public void Reset()
        {
            _gameObject.SetActive(true);
            _gameObject.transform.position = Vector3.zero;
            _playerMovement.Reset();
            _playerRotation.Reset();
        }

        public Vector2 GetCoordinates()
        {
            return _gameObject.transform.position;
        }
        public float GetAngle()
        {
            return _gameObject.transform.eulerAngles.z;
        }

        private void Update(float time)
        {
            _playerShooting.Update(time);
            _laserShooter.Update(time);
        }

        private void FixedUpdate(float time)
        {
            var inputVal = _inputMap.Player.Movement.ReadValue<Vector2>();
            _playerRotation.UpdateRotation(time, inputVal);
            _playerMovement.UpdateMovement(time, inputVal);
        }

        public void Hide()
        {
            _gameObject.SetActive(false);
        }
    }
}