using Player;

namespace UI
{
    public interface IPlayerContinuousInfoModelSetter
    {
        void SetProviders(IPlayerPositionProvider positionProvider,
            IPlayerSpeedProvider speedProvider,
            IPlayerLaserInfoProvider laserInfoProvider);
    }
}