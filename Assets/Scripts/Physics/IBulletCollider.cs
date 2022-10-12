using UnityEngine;

namespace Physics
{
    public interface IBulletCollider
    {
        bool IsActive { get; }
        BoxCollider2D GetCollider();
        void OnCollision();
        bool IsLaser { get; }
        
    }
}