using UnityEngine;

namespace Player
{
    [CreateAssetMenu(menuName = "Prefabs map")]
    public class PrefabsMap : ScriptableObject
    {
        [SerializeField] private GameObject playerObjectReference;
        [SerializeField] private GameObject bulletReference;
        [SerializeField] private GameObject laserReference;
        [SerializeField] private GameObject asteroidObjectReference;
        [SerializeField] private GameObject asteroidSmallObjectReference;
        [SerializeField] private GameObject ufoReference;
        [SerializeField] private GameObject enemyBulletReference;
        
        public GameObject PlayerObjectReference => playerObjectReference;
        public GameObject BulletReference => bulletReference;
        public GameObject LaserReference => laserReference;
        public GameObject AsteroidObjectReference => asteroidObjectReference;
        public GameObject AsteroidSmallObjectReference => asteroidSmallObjectReference;
        public GameObject UfoReference => ufoReference;
        public GameObject EnemyBulletReference => enemyBulletReference;
    }
}