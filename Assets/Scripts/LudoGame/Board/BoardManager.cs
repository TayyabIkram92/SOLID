using System.Collections.Generic;
using UnityEngine;
using LudoGame.Core;
using LudoGame.Tokens;

namespace LudoGame.Board
{
    /// <summary>
    /// Manages the board layout and position tracking
    /// </summary>
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private BoardData boardData;
        [SerializeField] private Transform boardTransform;

        private Dictionary<int, Vector2> _positionCoordinates = new Dictionary<int, Vector2>();
        private Dictionary<TokenColor, int> _startingPositions = new Dictionary<TokenColor, int>();
        private Dictionary<TokenColor, List<int>> _safePositions = new Dictionary<TokenColor, List<int>>();
        private List<int> _commonSafePositions = new List<int>();

        private void Awake()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Initialize starting positions for each color
            _startingPositions[TokenColor.Red] = boardData.redStartPosition;
            _startingPositions[TokenColor.Blue] = boardData.blueStartPosition;
            _startingPositions[TokenColor.Green] = boardData.greenStartPosition;
            _startingPositions[TokenColor.Yellow] = boardData.yellowStartPosition;

            // Initialize safe positions
            _safePositions[TokenColor.Red] = new List<int>(boardData.redSafePositions);
            _safePositions[TokenColor.Blue] = new List<int>(boardData.blueSafePositions);
            _safePositions[TokenColor.Green] = new List<int>(boardData.greenSafePositions);
            _safePositions[TokenColor.Yellow] = new List<int>(boardData.yellowSafePositions);

            // Initialize common safe positions
            _commonSafePositions = new List<int>(boardData.commonSafePositions);

            // Initialize position coordinates
            for (int i = 0; i < boardData.positionCoordinates.Length; i++)
            {
                _positionCoordinates[i] = boardData.positionCoordinates[i].position;
            }
        }

        public int GetStartingPosition(TokenColor color)
        {
            return _startingPositions[color];
        }

        public Vector2 GetPositionCoordinates(int position, TokenColor color)
        {
            // Handle final approach positions (color-specific)
            if (position >= 100)
            {
                // These are home-stretch positions, color-specific
                int colorIndex = (int)color;
                int homePosition = position - 100;
                return boardData.HomeStretchPositions[colorIndex][homePosition];
            }

            return _positionCoordinates[position];
        }

        public Vector2 GetHomeCoordinates(TokenColor color, int tokenIndex)
        {
            return boardData.HomePositions[(int)color][tokenIndex];
        }

        public Vector2 GetFinalHomeCoordinates(TokenColor color, int tokenIndex)
        {
            return boardData.FinalHomePositions[(int)color][tokenIndex];
        }

        public bool IsSafePosition(int position, TokenColor color)
        {
            // Check if it's a common safe position
            if (_commonSafePositions.Contains(position))
                return true;

            // Check if it's a color-specific safe position
            if (_safePositions[color].Contains(position))
                return true;

            return false;
        }

        public bool IsHomeStretchPosition(int position)
        {
            // Home stretch positions are >= 100
            return position >= 100;
        }
    }
}