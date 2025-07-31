using System.Collections.Generic;
using UnityEngine;
using LudoGame.Tokens;
using LudoGame.Rules;
using LudoGame.Core;

namespace LudoGame.Players
{
    /// <summary>
    /// Implementation of a human player controlled by user input
    /// </summary>
    public class HumanPlayer : Player
    {
        private Move _selectedMove = null;

        public HumanPlayer(TokenColor color, GameManager manager, IGameRules rules) : base(color, manager, rules)
        {
        }

        public override Move DecideMove(List<Move> validMoves)
        {
            // In the real implementation, this would wait for player input
            // For now, we'll just return the first valid move for simplicity
            if (_selectedMove != null)
            {
                Move move = _selectedMove;
                _selectedMove = null;
                return move;
            }

            return validMoves[0]; // Default to first move if none selected
        }

        public override bool ShouldRoll()
        {
            // Human players should always roll when it's their turn
            return true;
        }

        public void SelectMove(Move move)
        {
            _selectedMove = move;
        }
    }
}
