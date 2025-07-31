using System.Collections.Generic;
using UnityEngine;
using LudoGame.Tokens;
using LudoGame.Players;

namespace LudoGame.Rules
{
    /// <summary>
    /// Interface defining the rules of the Ludo game
    /// </summary>
    public interface IGameRules
    {
        bool IsValidMove(Token token, int diceValue);
        bool IsSafePosition(int position, TokenColor color);
        bool CanKillToken(int position, TokenColor attackerColor);
        List<Move> GetValidMoves(Player player, int diceValue);
        bool HasTokenReachedEnd(Token token);
        Vector2 GetPositionCoordinates(int position, TokenColor color);
        int GetNextPosition(Token token, int steps);
    }
}
