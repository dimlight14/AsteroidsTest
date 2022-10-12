using UnityEngine;

namespace Physics
{
    public interface ISimpleCollider
    {
        bool IsActive { get; }
        BoxCollider2D GetCollider();
        void OnCollision();
    }
}