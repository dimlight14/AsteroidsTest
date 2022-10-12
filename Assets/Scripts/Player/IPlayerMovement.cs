using UnityEngine;

namespace Player
{
    public interface IPlayerMovement
    {
        void SetMaxSpeedAndAcceleration(float maxSpeed, float acceleration);
        void UpdateMovement(float time, Vector2 inputValue);
        void Reset();
    }
}