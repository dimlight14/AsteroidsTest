using System;
using Main;
using Player;
using UnityEngine;

namespace UFO
{
    public class UFOController
    {
        private readonly GameObject _gameobject;
        private readonly GameObject _playerObject;
        private readonly Updater _updater;
        private readonly Action<UFOController> _despawnAction;
        private readonly IPlayerShooting _playerShooting;
        private readonly IPlayerMovement _playerMovement;
        private readonly IPlayerRotation _playerRotation;
        private readonly IBountyComponent _bountyComponent;

        public UFOController(
            GameObject gameobject,
            GameObject playerObject,
            Updater updater,
            Action<UFOController> despawnAction,
            IPlayerShooting playerShooting,
            IPlayerMovement playerMovement,
            IPlayerRotation playerRotation,
            IBountyComponent bountyComponent)
        {
            _gameobject = gameobject;
            _playerObject = playerObject;
            _updater = updater;
            _despawnAction = despawnAction;
            _playerShooting = playerShooting;
            _playerMovement = playerMovement;
            _playerRotation = playerRotation;
            _bountyComponent = bountyComponent;
        }

        public void GetWrecked(bool laserHit)
        {
            _bountyComponent.Claim();
            Despawn();
        }

        public void Initialize()
        {
            _gameobject.SetActive(true);
            _updater.OnUpdate += Update;
            _updater.OnFixedUpdate += FixedUpdate;
            _playerShooting.StartShooting();
        }

        private void Update(float time)
        {
            _playerShooting.Update(time);
        }

        private void FixedUpdate(float time)
        {
            _playerRotation.UpdateRotation(time, new Vector2(GetRotation(), 0));
            _playerMovement.UpdateMovement(time, Vector2.up);
        }

        private int GetRotation()
        {
            var toVector = _playerObject.transform.position - _gameobject.transform.position;
            toVector.Normalize();
            var dotProduct = Vector3.Dot(toVector, _gameobject.transform.right);

            var threshold = 0.015f;
            if (dotProduct < threshold && dotProduct > -threshold)
            {
                return 0;
            }

            return dotProduct > 0 ? 1 : -1;
        }

        private void Despawn()
        {
            _despawnAction.Invoke(this);
        }

        public void HideAndDisable()
        {
            _updater.OnUpdate -= Update;
            _updater.OnFixedUpdate -= FixedUpdate;
            _gameobject.SetActive(false);
        }
    }
}