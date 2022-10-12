using UnityEngine;

namespace Physics
{
    public interface IEnemyCollider
    {
        bool IsActive { get; }
        BoxCollider2D GetCollider();
        void GetWrecked(bool laserHit);
    }
}