using UnityEngine;
using UnityEngine.UI;
using LudoGame.Core;
using LudoGame.Rules;
using LudoGame.Players;

namespace LudoGame.UI
{
    /// <summary>
    /// Manages in-game UI elements and interactions
    /// </summary>
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Button diceButton;
        [SerializeField] private Text diceValueText;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();

            // Add listener to dice button
            if (diceButton != null)
                diceButton.onClick.AddListener(OnDiceClicked);
        }

        private void OnDiceClicked()
        {
            // Trigger dice roll in GameManager
            if (_gameManager != null)
            {
                _gameManager.RollDice();
            }
        }

        public void UpdateDiceValue(int value)
        {
            if (diceValueText != null)
            {
                diceValueText.text = value.ToString();
            }
        }
    }
}
