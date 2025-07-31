using System.Collections.Generic;
using UnityEngine;
using LudoGame.Tokens;
using LudoGame.Rules;
using LudoGame.Core;

namespace LudoGame.Board
{
    /// <summary>
    /// Handles visual representation of the board and tokens
    /// </summary>
    public class BoardRenderer : MonoBehaviour, IBoardRenderer
    {
        [SerializeField] private GameObject highlightPrefab;
        [SerializeField] private Transform highlightsParent;
        
        private Dictionary<Token, GameObject> _tokenObjects = new Dictionary<Token, GameObject>();
        private List<GameObject> _highlights = new List<GameObject>();
        private BoardManager _boardManager;
        
        private void Awake()
        {
            _boardManager = GetComponent<BoardManager>();
        }
        
        public void RegisterTokenObject(Token token, GameObject tokenObject)
        {
            _tokenObjects[token] = tokenObject;
        }
        
        public void UpdateTokenPosition(Token token, Vector2 position)
        {
            if (!_tokenObjects.ContainsKey(token))
            {
                Debug.LogError($"Token object not registered for {token.Color}");
                return;
            }
            
            // Convert Vector2 to Vector3 for Unity positioning
            Vector3 targetPosition = new Vector3(position.x, position.y, 0);
            
            // Move token to new position with smooth animation
            StartCoroutine(MoveTokenSmoothly(_tokenObjects[token], targetPosition));
        }
        
        private System.Collections.IEnumerator MoveTokenSmoothly(GameObject tokenObject, Vector3 targetPosition)
        {
            Vector3 startPosition = tokenObject.transform.localPosition;
            float journeyLength = Vector3.Distance(startPosition, targetPosition);
            float speed = 5f; // Units per second
            
            float startTime = Time.time;
            float distanceCovered = 0f;
            
            while (distanceCovered < journeyLength)
            {
                float fractionOfJourney = distanceCovered / journeyLength;
                tokenObject.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
                
                // Calculate distance covered this frame
                distanceCovered = (Time.time - startTime) * speed;
                
                yield return null;
            }
            
            // Ensure final position is exact
            tokenObject.transform.localPosition = targetPosition;
        }
        
        public void HighlightValidMoves(List<int> positions)
        {
            ClearHighlights();
            
            foreach (int position in positions)
            {
                // Instantiate highlight at position
                GameObject highlight = Instantiate(highlightPrefab, highlightsParent);
                
                // Get board position
                Vector2 pos = _boardManager.GetPositionCoordinates(position, TokenColor.Red); // Color doesn't matter for highlighting
                highlight.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                
                _highlights.Add(highlight);
            }
        }
        
        // New method for human player move selection
        public void HighlightValidMovesForHuman(List<Move> moves, GameManager gameManager)
        {
            ClearHighlights();
            
            foreach (Move move in moves)
            {
                // Instantiate highlight at position
                GameObject highlight = Instantiate(highlightPrefab, highlightsParent);
                
                // Get board position
                Vector2 pos = _boardManager.GetPositionCoordinates(move.TargetPosition, move.Token.Color);
                highlight.transform.localPosition = new Vector3(pos.x, pos.y, 0);
                
                // Add clickable component
                ClickableHighlight clickable = highlight.AddComponent<ClickableHighlight>();
                clickable.Initialize(move, gameManager);
                
                // Add collider for clicking (if not already on prefab)
                if (highlight.GetComponent<Collider2D>() == null)
                {
                    CircleCollider2D collider = highlight.AddComponent<CircleCollider2D>();
                    collider.radius = 0.5f; // Adjust based on your highlight size
                }
                
                _highlights.Add(highlight);
            }
        }
        
        public void ClearHighlights()
        {
            foreach (GameObject highlight in _highlights)
            {
                Destroy(highlight);
            }
            
            _highlights.Clear();
        }
    }
}