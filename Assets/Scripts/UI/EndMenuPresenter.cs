using System;

namespace UI
{
    public class EndMenuPresenter
    {
        private readonly IEndMenuView _endMenuView;
        private readonly IFinalScoresModel _scoresModel;

        public EndMenuPresenter(IEndMenuView endMenuView, IFinalScoresModel scoresModel, Action onRestart)
        {
            _endMenuView = endMenuView;
            _scoresModel = scoresModel;
            _endMenuView.Initialize(onRestart);
        }

        public void Hide()
        {
            _endMenuView.Hide();
        }

        public void Show()
        {
            _endMenuView.Show(_scoresModel.GetScore());
        }
    }
}