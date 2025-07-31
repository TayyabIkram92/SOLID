using UnityEngine;
using LudoGame.Core;

namespace LudoGame.Dice
{
    /// <summary>
    /// Controls dice rolling logic and visual representation
    /// </summary>
    public class DiceController : MonoBehaviour, IDiceRoller
    {
        [SerializeField] private Sprite[] diceSprites = new Sprite[6];
        [SerializeField] private SpriteRenderer diceRenderer;
        [SerializeField] private GameManager gameManager;

        private int _lastRoll = 0;
        private bool _canRoll = false;

        private void OnMouseDown()
        {
            if (_canRoll)
            {
                gameManager.RollDice();
            }
        }

        public int Roll()
        {
            // Generate random number between 1-6
            _lastRoll = Random.Range(1, 7);

            // Update dice sprite
            diceRenderer.sprite = diceSprites[_lastRoll - 1];

            // Disable rolling until next turn
            _canRoll = false;

            return _lastRoll;
        }

        public int GetLastRoll()
        {
            return _lastRoll;
        }

        public void EnableRolling(bool enabled)
        {
            _canRoll = enabled;
        }
    }
}
