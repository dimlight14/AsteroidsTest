using UnityEngine;

namespace Player
{
    public interface IPlayerRotation
    {
        void UpdateRotation(float time, Vector2 input);
        void Reset();
    }
}