using System;

namespace Main
{
    public class BountyComponent : IBountyComponent
    {
        private readonly int _bounty;
        private readonly Action<int> _claimAction;

        public BountyComponent(int bounty, Action<int> claimAction)
        {
            _bounty = bounty;
            _claimAction = claimAction;
        }

        public void Claim()
        {
            _claimAction?.Invoke(_bounty);
        }
    }
}