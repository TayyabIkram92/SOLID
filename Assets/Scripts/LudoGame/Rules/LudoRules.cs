using System.Collections.Generic;
using UnityEngine;
using LudoGame.Board;
using LudoGame.Tokens;
using LudoGame.Core;
using LudoGame.Players;

namespace LudoGame.Rules
{
    /// <summary>
    /// Implementation of standard Ludo game rules
    /// </summary>
    public class LudoRules : MonoBehaviour, IGameRules
    {
        [SerializeField] private GameSettings gameSettings;
        private BoardManager _boardManager;

        private void Awake()
        {
            _boardManager = GetComponent<BoardManager>();
        }

        public bool IsValidMove(Token token, int diceValue)
        {
            // Can't move tokens that have reached the end
            if (token.HasReachedEnd)
                return false;

            // If token is in home, can only move out with a 6
            if (token.IsInHome)
                return diceValue == gameSettings.valueToExitHome;

            // If token is in play, check if move is within bounds
            if (token.IsInPlay)
            {
                int nextPosition = GetNextPosition(token, diceValue);

                // Check if token will overshoot the final position
                if (IsHomeStretchPosition(token.CurrentPosition))
                {
                    // For home stretch, check if we're exceeding the end (position 104)
                    return nextPosition <= 104;
                }

                return true; // Regular moves are valid
            }

            return false;
        }

        public bool IsSafePosition(int position, TokenColor color)
        {
            return _boardManager.IsSafePosition(position, color);
        }

        public bool CanKillToken(int position, TokenColor attackerColor)
        {
            // Can't kill on safe positions
            if (IsSafePosition(position, attackerColor))
                return false;

            // Can't kill in home stretch
            if (IsHomeStretchPosition(position))
                return false;

            return true;
        }

        public List<Move> GetValidMoves(Player player, int diceValue)
        {
            List<Move> validMoves = new List<Move>();
            List<Token> tokens = player.GetTokens();

            foreach (Token token in tokens)
            {
                if (IsValidMove(token, diceValue))
                {
                    int targetPosition;

                    // If token is in home and dice value is 6, move to starting position
                    if (token.IsInHome && diceValue == gameSettings.valueToExitHome)
                    {
                        targetPosition = _boardManager.GetStartingPosition(token.Color);
                    }
                    else
                    {
                        targetPosition = GetNextPosition(token, diceValue);
                    }

                    // Check if this move will kill another token
                    bool willKill = false;
                    if (!IsHomeStretchPosition(targetPosition) && !IsSafePosition(targetPosition, token.Color))
                    {
                        // Check all other players' tokens
                        foreach (Player otherPlayer in player.GetOtherPlayers())
                        {
                            if (otherPlayer.HasSingleTokenAt(targetPosition))
                            {
                                willKill = true;
                                break;
                            }
                        }
                    }

                    validMoves.Add(new Move(token, diceValue, targetPosition, willKill));
                }
            }

            return validMoves;
        }

        public bool HasTokenReachedEnd(Token token)
        {
            // A token has reached the end when it's at position 104 (final home position)
            return token.CurrentPosition == 104;
        }

        public Vector2 GetPositionCoordinates(int position, TokenColor color)
        {
            return _boardManager.GetPositionCoordinates(position, color);
        }

        public int GetNextPosition(Token token, int steps)
        {
            if (token.IsInHome)
            {
                // If in home and rolling a 6, go to starting position
                if (steps == gameSettings.valueToExitHome)
                {
                    return _boardManager.GetStartingPosition(token.Color);
                }
                return -1; // Invalid move
            }

            int currentPos = token.CurrentPosition;

            // If already in home stretch
            if (IsHomeStretchPosition(currentPos))
            {
                // Move further along home stretch
                return currentPos + steps;
            }

            int nextPos = (currentPos + steps) % 52; // Wrap around the board

            // Check if we need to enter home stretch
            int startPos = _boardManager.GetStartingPosition(token.Color);
            int homeEntryPos = (startPos + 51) % 52; // Position just before home stretch

            // If we crossed or landed on the home entry position, enter home stretch
            if (CrossedPosition(currentPos, nextPos, homeEntryPos))
            {
                // Calculate how many steps into home stretch
                int stepsAfterHomeEntry;

                if (nextPos < currentPos) // Wrapped around the board
                {
                    stepsAfterHomeEntry = nextPos + 52 - homeEntryPos;
                }
                else
                {
                    stepsAfterHomeEntry = nextPos - homeEntryPos;
                }

                // First home stretch position is 100
                return 100 + stepsAfterHomeEntry - 1;
            }

            return nextPos;
        }

        private bool CrossedPosition(int start, int end, int position)
        {
            // Check if the move from start to end crossed or landed on the position
            if (start <= end) // Normal move
            {
                return position >= start && position <= end;
            }
            else // Wrapped around the board
            {
                return position >= start || position <= end;
            }
        }

        private bool IsHomeStretchPosition(int position)
        {
            return _boardManager.IsHomeStretchPosition(position);
        }
    }
}
