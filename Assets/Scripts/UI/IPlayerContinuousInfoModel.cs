using UnityEngine;

namespace UI
{
    public interface IPlayerContinuousInfoModel
    {
        Vector2 GetCoordinates();
        float GetAngle();
        float GetSpeed();
        int GetLaserShots();
        float GetLaserReloadRemaining();
    }
}