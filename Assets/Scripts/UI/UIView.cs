using TMPro;
using UnityEngine;

namespace UI
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coordinatesText;
        [SerializeField] private TextMeshProUGUI angleText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI shotsRemainingText;
        [SerializeField] private TextMeshProUGUI reloadText;
        [SerializeField] private TextMeshProUGUI scoreText;
        public void UpdateCoordinates(Vector2 coordinates)
        {
            coordinatesText.text = coordinates.ToString();
        }

        public void UpdateAngle(float angle)
        {
            angleText.text = angle.ToString();
        }

        public void UpdateSpeed(float speed)
        {
            speedText.text = speed.ToString();
        }

        public void UpdateNumberOfShots(int shotsRem)
        {
            shotsRemainingText.text = shotsRem.ToString();
        }

        public void UpdateReloadRemaining(string reloadRem)
        {
            reloadText.text = reloadRem;
        }

        public void ChangeScore(int newScore)
        {
            scoreText.text = newScore.ToString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}