using System.Collections.Generic;
using UnityEngine;
using LudoGame.Board;

namespace LudoGame.Tokens
{
    /// <summary>
    /// Manages the creation and visual representation of tokens
    /// </summary>
    public class TokenManager : MonoBehaviour
    {
        [SerializeField] private GameObject redTokenPrefab;
        [SerializeField] private GameObject blueTokenPrefab;
        [SerializeField] private GameObject greenTokenPrefab;
        [SerializeField] private GameObject yellowTokenPrefab;

        [SerializeField] private BoardManager boardManager;
        [SerializeField] private BoardRenderer boardRenderer;

        [SerializeField] private Transform tokensParent;

        public void CreateTokenObject(Token token)
        {
            GameObject tokenPrefab = GetTokenPrefab(token.Color);
            GameObject tokenObject = Instantiate(tokenPrefab, tokensParent);

            // Set initial position to home
            Vector2 homePosition = boardManager.GetHomeCoordinates(token.Color, token.TokenIndex);
            tokenObject.transform.localPosition = new Vector3(homePosition.x, homePosition.y, 0);

            // Register token with renderer
            boardRenderer.RegisterTokenObject(token, tokenObject);
        }

        private GameObject GetTokenPrefab(TokenColor color)
        {
            switch (color)
            {
                case TokenColor.Red:
                    return redTokenPrefab;
                case TokenColor.Blue:
                    return blueTokenPrefab;
                case TokenColor.Green:
                    return greenTokenPrefab;
                case TokenColor.Yellow:
                    return yellowTokenPrefab;
                default:
                    return redTokenPrefab;
            }
        }
    }
}