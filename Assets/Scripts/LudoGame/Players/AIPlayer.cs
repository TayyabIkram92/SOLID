using System.Collections.Generic;
using LudoGame.Rules;
using LudoGame.Tokens;
using LudoGame.Core;
using LudoGame.AI;

namespace LudoGame.Players
{
    /// <summary>
    /// Implementation of an AI player using a strategy pattern
    /// </summary>
    public class AIPlayer : Player
    {
        private AIStrategy _strategy;

        public AIPlayer(TokenColor color, GameManager manager, IGameRules rules, AIStrategy aiStrategy) 
            : base(color, manager, rules)
        {
            _strategy = aiStrategy;
        }

        public override Move DecideMove(List<Move> validMoves)
        {
            // Use the AI strategy to determine the best move
            return _strategy.CalculateBestMove(this, validMoves);
        }

        public override bool ShouldRoll()
        {
            // AI always rolls when it's their turn
            return true;
        }
    }
}
