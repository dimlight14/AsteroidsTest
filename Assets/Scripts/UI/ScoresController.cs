using System;

namespace UI
{
    public class ScoresController : IScoresModel, IFinalScoresModel
    {
        private int _currentScore = 0;
        public event Action<int> OnScoreChanged;

        public void Reset()
        {
            _currentScore = 0;
            OnScoreChanged?.Invoke(_currentScore);
        }
        public void ClaimBounty(int bounty)
        {
            _currentScore += bounty;
            OnScoreChanged?.Invoke(_currentScore);
        }

        public int GetScore()
        {
            return _currentScore;
        }
    }
}