using UnityEngine;
using UnityEngine.UI;
using LudoGame.Core;
using LudoGame.Tokens;

namespace LudoGame.UI
{
    /// <summary>
    /// Manages UI elements and interactions
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject victoryScreen;
        [SerializeField] private Text victoryText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Text turnIndicatorText;
        [SerializeField] private Image turnIndicatorColor;

        [SerializeField] private Color redColor = Color.red;
        [SerializeField] private Color blueColor = Color.blue;
        [SerializeField] private Color greenColor = Color.green;
        [SerializeField] private Color yellowColor = Color.yellow;

        private GameManager _gameManager;

        private void Awake()
        {
            // Find GameManager
            _gameManager = FindObjectOfType<GameManager>();

            // Hide victory screen initially
            if (victoryScreen != null)
                victoryScreen.SetActive(false);

            // Add listener to restart button
            if (restartButton != null)
                restartButton.onClick.AddListener(OnRestartClicked);
        }

        public void ShowVictoryScreen(bool playerWon)
        {
            if (victoryScreen == null || victoryText == null)
                return;

            victoryScreen.SetActive(true);

            if (playerWon)
            {
                victoryText.text = "You Won!";
            }
            else
            {
                victoryText.text = "You Lost!";
            }
        }

        public void HideVictoryScreen()
        {
            if (victoryScreen != null)
                victoryScreen.SetActive(false);
        }

        public void UpdateTurnIndicator(TokenColor currentPlayerColor)
        {
            if (turnIndicatorText == null || turnIndicatorColor == null)
                return;

            switch (currentPlayerColor)
            {
                case TokenColor.Red:
                    turnIndicatorText.text = "Red's Turn";
                    turnIndicatorColor.color = redColor;
                    break;
                case TokenColor.Blue:
                    turnIndicatorText.text = "Blue's Turn";
                    turnIndicatorColor.color = blueColor;
                    break;
                case TokenColor.Green:
                    turnIndicatorText.text = "Green's Turn";
                    turnIndicatorColor.color = greenColor;
                    break;
                case TokenColor.Yellow:
                    turnIndicatorText.text = "Yellow's Turn";
                    turnIndicatorColor.color = yellowColor;
                    break;
            }
        }

        private void OnRestartClicked()
        {
            if (_gameManager != null)
            {
                _gameManager.RestartGame();
            }
        }
    }
}
