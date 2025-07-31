using System.Collections.Generic;
using UnityEngine;
using LudoGame.Tokens;
using LudoGame.Players;

namespace LudoGame.Rules
{
    /// <summary>
    /// Validates moves according to game rules
    /// </summary>
    public class MoveValidator : MonoBehaviour
    {
        private IGameRules _gameRules;

        private void Awake()
        {
            _gameRules = GetComponent<IGameRules>();
        }

        public List<Move> FilterMovesByPriority(List<Move> moves)
        {
            List<Move> prioritizedMoves = new List<Move>();

            // First priority: Moves that will kill an opponent
            foreach (Move move in moves)
            {
                if (move.WillKill)
                {
                    prioritizedMoves.Add(move);
                }
            }

            if (prioritizedMoves.Count > 0)
                return prioritizedMoves;

            // Second priority: Moves that bring tokens out from home
            foreach (Move move in moves)
            {
                if (move.Token.IsInHome)
                {
                    prioritizedMoves.Add(move);
                }
            }

            if (prioritizedMoves.Count > 0)
                return prioritizedMoves;

            // Third priority: Moves to safe positions
            foreach (Move move in moves)
            {
                if (_gameRules.IsSafePosition(move.TargetPosition, move.Token.Color))
                {
                    prioritizedMoves.Add(move);
                }
            }

            if (prioritizedMoves.Count > 0)
                return prioritizedMoves;

            // If no priorities found, return all moves
            return moves;
        }
    }
}
