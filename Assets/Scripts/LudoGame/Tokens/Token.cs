using UnityEngine;

namespace LudoGame.Tokens
{
    /// <summary>
    /// Represents an individual token in the Ludo game
    /// </summary>
    public class Token
    {
        public TokenColor Color { get; set; }
        public int CurrentPosition { get; set; }
        public bool IsInHome { get; set; }
        public bool IsInPlay { get; set; }
        public bool HasReachedEnd { get; set; }
        public int TokenIndex { get; set; } // 0-3 to identify which of the 4 same-colored tokens

        public Token(TokenColor color, int index)
        {
            Color = color;
            TokenIndex = index;
            IsInHome = true;
            IsInPlay = false;
            HasReachedEnd = false;
            CurrentPosition = -1; // Not on board yet
        }

        public void Reset()
        {
            IsInHome = true;
            IsInPlay = false;
            HasReachedEnd = false;
            CurrentPosition = -1;
        }
    }
}
