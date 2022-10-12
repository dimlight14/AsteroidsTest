using UnityEngine;

namespace Asteroids
{
    public interface IAsteroidsFactoryService
    {
        void SpawnSmallAsteroidsAroundPoint(Vector2 position);
        void DespawnAsteroid(AsteroidController asteroid);
    }
}