using System.Collections.Generic;
using UnityEngine;
using LudoGame.Rules;
using LudoGame.Players;

namespace LudoGame.AI
{
    /// <summary>
    /// Easy AI strategy - chooses moves randomly
    /// </summary>
    public class EasyAIStrategy : AIStrategy
    {
        public override Move CalculateBestMove(Player player, List<Move> moves)
        {
            // Easy AI just picks a random move
            if (moves.Count == 0)
                return null;

            int randomIndex = Random.Range(0, moves.Count);
            return moves[randomIndex];
        }
    }
}
