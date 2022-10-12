namespace Player
{
    public interface IPlayerShooting
    {
        void StopShooting();
        void StartShooting();
        void Update(float time);
    }
}