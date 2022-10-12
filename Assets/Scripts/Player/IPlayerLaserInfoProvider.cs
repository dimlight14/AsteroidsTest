namespace Player
{
    public interface IPlayerLaserInfoProvider
    {
        int GetShotsRemaining();
        float GetCooldownRemaining();
    }
}