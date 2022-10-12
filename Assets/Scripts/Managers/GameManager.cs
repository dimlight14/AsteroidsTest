using Asteroids;
using Bullets;
using Main;
using Physics;
using Player;
using UFO;
using UI;

namespace Managers
{
    public class GameManager
    {
        private readonly IFactoryCreator _factoryCreator;
        private readonly Updater _updater;
        private readonly UIView _uiView;
        private readonly EndMenuView _endMenuView;
        
        private IBulletsFactory _bulletsFactory;
        private IPlayerFactory _playerFactory;
        private IAsteroidsFactory _asteroidsFactory;
        private IUFOFactory _ufoFactory;
        private ICollisionDetectorInitializable _collisionDetector;
        private PlayerContinuousInfoModel _infoModel;
        private UIPresenter _uiPresenter;
        private EndMenuPresenter _endMenuPresenter;
        private ScoresController _scoresController;

        private PlayerController _playerController;
        private EnemySpawner _enemySpawner;

        public GameManager(IFactoryCreator factoryCreator, Updater updater, UIView uiView, EndMenuView endMenuView)
        {
            _factoryCreator = factoryCreator;
            _updater = updater;
            _uiView = uiView;
            _endMenuView = endMenuView;

            SetUI();
            Initialize();
            StartLevel();
        }

        private void SetUI()
        {
            _scoresController = new ScoresController();
            _infoModel = new PlayerContinuousInfoModel();
            _uiPresenter = new UIPresenter(_infoModel, _uiView, _updater, _scoresController);

            _endMenuPresenter = new EndMenuPresenter(_endMenuView, _scoresController, Restart);
            _endMenuPresenter.Hide();
        }

        private void Initialize()
        {
            _collisionDetector = _factoryCreator.GetCollisionDetector();
            _bulletsFactory = _factoryCreator.GetBulletFactory();
            _asteroidsFactory = _factoryCreator.GetAsteroidsFactory(_scoresController);
            _playerFactory = _factoryCreator.GetPlayerFactory(_bulletsFactory);
            _ufoFactory = _factoryCreator.GetUfoFactory(_bulletsFactory, _scoresController);

            _enemySpawner = new EnemySpawner(_ufoFactory, _asteroidsFactory);
            
            _updater.OnUpdate += Update;
        }

        private void StartLevel()
        {
            _collisionDetector.Initialize();
            _uiPresenter.Initialize();
            _scoresController.Reset();

            var playerContainer = _playerFactory.CreatePlayer();
            _playerController = playerContainer.Controller;
            _infoModel.SetProviders(playerContainer.Controller, playerContainer.Movement, playerContainer.LaserShooter);
            _playerController.Reset();
            _playerController.Initialize();
            _playerController.OnPlayerDestroyed += LoseGame;
            
            _enemySpawner.Initialize(playerContainer.GameObject);
            
            _updater.SetActive(true);
        }

        private void Restart()
        {
            ClearAll();
            _uiPresenter.Show();
            _playerController.Reset();
            _updater.SetActive(true);
            _enemySpawner.Reset();
        }

        private void ClearAll()
        {
            _endMenuPresenter.Hide();
            _scoresController.Reset();
            _asteroidsFactory.ClearAll();
            _ufoFactory.ClearAll();
            _bulletsFactory.ClearAll();
            _collisionDetector.ClearAll();
        }

        private void LoseGame()
        {
            _playerController.Hide();
            _updater.SetActive(false);
            _endMenuPresenter.Show();
            _uiPresenter.Hide();
        }

        private void Update(float time)
        {
            _enemySpawner.Update(time);
        }
    }
}