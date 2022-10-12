using UnityEngine;

namespace UFO
{
    public interface IUFOFactory
    {
        int ActiveUfo { get; }
        void SpawnAt(Vector2 position, GameObject playerController);
        void ClearAll();
    }
}