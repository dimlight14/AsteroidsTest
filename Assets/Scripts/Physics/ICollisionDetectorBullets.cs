namespace Physics
{
    public interface ICollisionDetectorBullets
    {
        void RegisterBullet(IBulletCollider bulletScript);
        void RegisterEnemyBullet(IBulletCollider bulletScript);
        void UnregisterBullet(IBulletCollider bulletScript);
    }
}