using System.Collections.Generic;
using LudoGame.Rules;
using LudoGame.Players;

namespace LudoGame.AI
{
    /// <summary>
    /// Abstract base class for AI decision-making strategies
    /// </summary>
    public abstract class AIStrategy
    {
        public abstract Move CalculateBestMove(Player player, List<Move> moves);
    }
}
