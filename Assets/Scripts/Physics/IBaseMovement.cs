using UnityEngine;

namespace Physics
{
    public interface IBaseMovement
    {
        void SetSpeed(float speed);
        void SetDirection(Vector2 direction);
        void MoveForward(float time);
    }
}