using UnityEngine;
using UnityEngine.UI;
using LudoGame.Core;

namespace LudoGame.UI
{
    /// <summary>
    /// Manages the victory screen and restart functionality
    /// </summary>
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Text victoryText;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();

            // Add listener to restart button
            if (restartButton != null)
                restartButton.onClick.AddListener(OnRestartClicked);

            // Hide initially
            gameObject.SetActive(false);
        }

        public void Show(bool playerWon)
        {
            gameObject.SetActive(true);

            if (victoryText != null)
            {
                victoryText.text = playerWon ? "You Won!" : "You Lost!";
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnRestartClicked()
        {
            if (_gameManager != null)
            {
                _gameManager.RestartGame();
                Hide();
            }
        }
    }
}
