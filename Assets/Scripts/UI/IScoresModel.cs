using System;

namespace UI
{
    public interface IScoresModel
    {
        event Action<int> OnScoreChanged;
    }
}