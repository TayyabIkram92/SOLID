using UnityEngine;

namespace LudoGame.Core
{
    /// <summary>
    /// Contains configurable game settings
    /// </summary>
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Ludo/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Timers")]
        [Tooltip("Time in seconds for human player to roll dice")]
        public float rollTimer = 10f;

        [Tooltip("Time in seconds for human player to make a move")]
        public float moveTimer = 15f;

        [Tooltip("Delay in seconds before AI rolls dice")]
        public float aiRollDelay = 2f;

        [Tooltip("Delay in seconds before AI makes a move")]
        public float aiMoveDelay = 3f;

        [Header("Game Rules")]
        [Tooltip("Maximum number of consecutive sixes allowed before turn is skipped")]
        public int maxConsecutiveSixes = 3;

        [Tooltip("Dice value required to bring a token out from home")]
        public int valueToExitHome = 6;
    }
}
