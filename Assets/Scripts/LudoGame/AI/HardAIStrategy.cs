using System.Collections.Generic;
using UnityEngine;
using LudoGame.Rules;
using LudoGame.Players;
using LudoGame.Tokens;

namespace LudoGame.AI
{
    /// <summary>
    /// Hard AI strategy - makes strategic decisions including blocking and optimal positioning
    /// </summary>
    public class HardAIStrategy : AIStrategy
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

            // Priority 2: Move tokens that are in danger to safe positions
            List<Move> safetyMoves = new List<Move>();
            foreach (Move move in moves)
            {
                // Check if current position is unsafe and target position is safe
                if (IsTokenInDanger(move.Token, player) && IsSafePosition(move.TargetPosition))
                {
                    safetyMoves.Add(move);
                }
            }

            if (safetyMoves.Count > 0)
            {
                int randomIndex = Random.Range(0, safetyMoves.Count);
                return safetyMoves[randomIndex];
            }

            // Priority 3: Bring tokens out from home
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

            // Priority 4: Move tokens that are furthest along
            Move bestMove = GetMostAdvancedTokenMove(moves, player.Color);
            if (bestMove != null)
            {
                return bestMove;
            }

            // Fallback: Random move
            int index = Random.Range(0, moves.Count);
            return moves[index];
        }

        private bool IsTokenInDanger(Token token, Player player)
        {
            // Token is in danger if it's not in a safe position and there's only one token there
            if (token.IsInHome || !token.IsInPlay || IsSafePosition(token.CurrentPosition))
                return false;

            // Check if this is the only token at this position
            int tokensAtPosition = 0;
            foreach (Token t in player.GetTokens())
            {
                if (t.CurrentPosition == token.CurrentPosition && t.IsInPlay)
                {
                    tokensAtPosition++;
                }
            }

            return tokensAtPosition == 1;
        }

        private bool IsSafePosition(int position)
        {
            // For simplicity, we'll consider standard safe positions
            // In a real implementation, this would use the IGameRules interface
            int[] safePositions = { 0, 8, 13, 21, 26, 34, 39, 47 };
            foreach (int safePos in safePositions)
            {
                if (position == safePos || position >= 100) // Home stretch is also safe
                    return true;
            }
            return false;
        }

        private Move GetMostAdvancedTokenMove(List<Move> moves, TokenColor color)
        {
            // Prioritize moves for tokens that are furthest along the board
            Move bestMove = null;
            int highestProgress = -1;

            foreach (Move move in moves)
            {
                // Calculate progress based on position
                int progress = CalculateProgress(move.TargetPosition, color);

                if (progress > highestProgress)
                {
                    highestProgress = progress;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        private int CalculateProgress(int position, TokenColor color)
        {
            // Calculate how far along the token is (higher is better)
            // Home stretch positions have highest priority
            if (position >= 100)
                return 100 + position;

            // Calculate based on how close to entering home stretch
            int startingPosition = GetStartingPosition(color);
            int homeEntryPosition = (startingPosition + 51) % 52;

            // Calculate distance from starting position to home entry
            int distanceToHomeEntry;
            if (position >= startingPosition && position <= homeEntryPosition)
            {
                distanceToHomeEntry = position - startingPosition;
            }
            else if (position < startingPosition)
            {
                distanceToHomeEntry = (position + 52) - startingPosition;
            }
            else
            {
                distanceToHomeEntry = position - startingPosition;
            }

            return distanceToHomeEntry;
        }

        private int GetStartingPosition(TokenColor color)
        {
            // Return starting position for each color
            switch (color)
            {
                case TokenColor.Red:
                    return 0;
                case TokenColor.Blue:
                    return 13;
                case TokenColor.Green:
                    return 26;
                case TokenColor.Yellow:
                    return 39;
                default:
                    return 0;
            }
        }
    }
}
