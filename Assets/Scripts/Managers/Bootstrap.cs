using Main;
using Player;
using UI;
using UnityEngine;

namespace Managers
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Updater updater;
        [SerializeField] private PrefabsMap prefabsMap;
        [SerializeField] private UIView uiView;
        [SerializeField] private EndMenuView endMenuView;

        private GameManager _gameManager;
        private void Start()
        {
            var factoryCreator = new FactoryCreator(prefabsMap, mainCamera, updater);
            _gameManager = new GameManager(factoryCreator, updater,uiView, endMenuView);
        }
    }
}