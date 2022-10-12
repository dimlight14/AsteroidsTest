using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndMenuView : MonoBehaviour, IEndMenuView
    {
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private Button restartButton;

        private Action _onRestartClicked;
        
        public void Initialize(Action onRestartClicked)
        {
            _onRestartClicked = onRestartClicked;
        }

        public void RestartClick()
        {
            _onRestartClicked?.Invoke();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(int score)
        {
            gameObject.SetActive(true);
            finalScoreText.text = score.ToString();
        }
    }
}