using UnityEngine;

namespace Asteroids
{
    public interface IAsteroidsFactory
    {
        int ActiveAsteroids { get; }
        void SpawnAsteroidAt(Vector2 position);
        void ClearAll();
    }
}