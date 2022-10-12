using System;

namespace UI
{
    public interface IEndMenuView
    {
        void Initialize(Action onRestartClicked);
        void Hide();
        void Show(int score);
    }
}