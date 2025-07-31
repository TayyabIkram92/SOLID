
using UnityEngine;
using LudoGame.Rules;
using LudoGame.Core;

namespace LudoGame.Board
{
    /// <summary>
    /// Handles click interactions on highlighted positions for human player moves
    /// </summary>
    public class ClickableHighlight : MonoBehaviour
    {
        private Move _associatedMove;
        private GameManager _gameManager;
        
        public void Initialize(Move move, GameManager manager)
        {
            _associatedMove = move;
            _gameManager = manager;
        }
        
        private void OnMouseDown()
        {
            if (_gameManager != null && _associatedMove != null)
            {
                // Tell the game manager that this move was selected
                _gameManager.OnMoveSelected(_associatedMove);
            }
        }
    }
}
