using UnityEngine;

namespace Player
{
    public interface IPlayerPositionProvider
    {
        Vector2 GetCoordinates();
        float GetAngle();
    }
}