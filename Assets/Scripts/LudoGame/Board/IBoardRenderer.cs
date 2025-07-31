using System.Collections.Generic;
using UnityEngine;
using LudoGame.Tokens;
using LudoGame.Rules;
using LudoGame.Core;

namespace LudoGame.Board
{
    /// <summary>
    /// Interface for rendering board elements and token movements
    /// </summary>
    public interface IBoardRenderer
    {
        void UpdateTokenPosition(Token token, Vector2 position);
        void HighlightValidMoves(List<int> positions);
        void HighlightValidMovesForHuman(List<Move> moves, GameManager gameManager);
        void ClearHighlights();
    }
}