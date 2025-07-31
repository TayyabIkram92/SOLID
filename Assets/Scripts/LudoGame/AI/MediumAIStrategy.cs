using System.Collections.Generic;
using UnityEngine;
using LudoGame.Rules;
using LudoGame.Players;

namespace LudoGame.AI
{
    /// <summary>
    /// Medium AI strategy - prioritizes killing and bringing tokens out
    /// </summary>
    public class MediumAIStrategy : AIStrategy
    {
        public override Move CalculateBestMove(Player player, List<Move> moves)
        {
            if (moves.Count == 0)
                return null;

            // Priority 1: Kill opponent tokens
            List<Move> killingMoves = new List<Move>();
            foreach (Move move in moves)
            {
                if (move.WillKill)
                {
                    killingMoves.Add(move);
                }
            }

            if (killingMoves.Count > 0)
            {
                int randomIndex = Random.Range(0, killingMoves.Count);
                return killingMoves[randomIndex];
            }

            // Priority 2: Bring tokens out from home
            List<Move> homeExitMoves = new List<Move>();
            foreach (Move move in moves)
            {
                if (move.Token.IsInHome)
                {
                    homeExitMoves.Add(move);
                }
            }

            if (homeExitMoves.Count > 0)
            {
                int randomIndex = Random.Range(0, homeExitMoves.Count);
                return homeExitMoves[randomIndex];
            }

            // Priority 3: Random move
            int index = Random.Range(0, moves.Count);
            return moves[index];
        }
    }
}
