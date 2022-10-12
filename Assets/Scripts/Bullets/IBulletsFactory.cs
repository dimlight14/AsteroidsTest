using UnityEngine;

namespace Bullets
{
    public interface IBulletsFactory
    {
        void SpawnBullet(Vector2 position, Vector2 direction, float rotation);
        void SpawnLaser(Vector2 position, Vector2 direction, float rotation);
        void SpawnEnemyBullet(Vector2 position, Vector2 direction, float rotation);
        void ClearAll();
    }
}