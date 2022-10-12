using Main;
using UnityEngine;

namespace UI
{
    public class UIPresenter
    {
        private readonly Updater _updater;
        private readonly IPlayerContinuousInfoModel _playerContinuous;
        private readonly UIView _uiView;
        private readonly IScoresModel _scoresModel;

        public UIPresenter(IPlayerContinuousInfoModel infoModel, UIView uiView, Updater updater,
            IScoresModel scoresModel)
        {
            _updater = updater;
            _playerContinuous = infoModel;
            _uiView = uiView;
            _scoresModel = scoresModel;

            _scoresModel.OnScoreChanged += ChangeScore;
        }

        private void ChangeScore(int newScore)
        {
            _uiView.ChangeScore(newScore);
        }

        public void Initialize()
        {
            _updater.OnUpdate += _ => Update();
        }

        private void Update()
        {
            var coordinates = _playerContinuous.GetCoordinates();
            coordinates = new Vector2(Mathf.Round(coordinates.x * 10) / 10, Mathf.Round(coordinates.y * 10) / 10);
            _uiView.UpdateCoordinates(coordinates);

            var angle = _playerContinuous.GetAngle();
            angle = Mathf.Round(angle);
            _uiView.UpdateAngle(angle);

            var speed = _playerContinuous.GetSpeed();
            speed = Mathf.Round(speed * 10);
            _uiView.UpdateSpeed(speed);

            var shotsRem = _playerContinuous.GetLaserShots();
            _uiView.UpdateNumberOfShots(shotsRem);

            var reloadRem = _playerContinuous.GetLaserReloadRemaining();
            reloadRem = 1 - Mathf.Clamp01(reloadRem);
            reloadRem = Mathf.Round(reloadRem * 100);
            _uiView.UpdateReloadRemaining(reloadRem + "%");
        }

        public void Hide()
        {
            _uiView.Hide();
        }

        public void Show()
        {
            _uiView.Show();
        }
    }
}