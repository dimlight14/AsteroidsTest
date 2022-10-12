using Player;
using UnityEngine;

namespace UI
{
    public class PlayerContinuousInfoModel : IPlayerContinuousInfoModel, IPlayerContinuousInfoModelSetter
    {
        private IPlayerPositionProvider _positionProvider;
        private IPlayerSpeedProvider _speedProvider;
        private IPlayerLaserInfoProvider _laserInfoProvider;

        public void SetProviders(IPlayerPositionProvider positionProvider,
            IPlayerSpeedProvider speedProvider,
            IPlayerLaserInfoProvider laserInfoProvider)
        {
            _positionProvider = positionProvider;
            _speedProvider = speedProvider;
            _laserInfoProvider = laserInfoProvider;
        }

        public Vector2 GetCoordinates()
        {
            return _positionProvider.GetCoordinates();
        }

        public float GetAngle()
        {
            return _positionProvider.GetAngle();
        }

        public float GetSpeed()
        {
            return _speedProvider.GetSpeed();
        }

        public int GetLaserShots()
        {
            return _laserInfoProvider.GetShotsRemaining();
        }

        public float GetLaserReloadRemaining()
        {
            return _laserInfoProvider.GetCooldownRemaining();
        }
    }
}