using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudoGame.Dice;
using LudoGame.Board;
using LudoGame.Players;
using LudoGame.AI;
using LudoGame.Rules;
using LudoGame.Systems;
using LudoGame.Tokens;
using LudoGame.UI;

namespace LudoGame.Core
{
    /// <summary>
    /// Manages the overall game state, turn management and victory conditions
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int humanPlayerIndex = 0;

        private GameState _currentState = GameState.WaitingForRoll;
        private List<Player> _players = new List<Player>();
        private int _currentPlayerIndex = 0;
        private int _consecutiveSixes = 0;
        private bool _gameOver = false;

        // Store current valid moves for human player selection
        private List<Move> _currentValidMoves = new List<Move>();
        private bool _waitingForHumanMoveSelection = false;

        private IDiceRoller _diceRoller;
        private IGameRules _gameRules;
        private IBoardRenderer _boardRenderer;
        private ITimerSystem _timerSystem;
        private IAudioSystem _audioSystem;
        private UIManager _uiManager;
        private TokenManager _tokenManager;

        // Dependency injection through Unity Inspector with [SerializeField]
        [SerializeField] private DiceController diceControllerPrefab;
        [SerializeField] private BoardManager boardManagerPrefab;
        [SerializeField] private UIManager uiManagerPrefab;
        [SerializeField] private TokenManager tokenManagerPrefab;

        private void Awake()
        {
            // Initialize all dependencies
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Create board
            // var boardManager = Instantiate(boardManagerPrefab);
            var boardManager = boardManagerPrefab;
            _gameRules = boardManager.GetComponent<LudoRules>();
            _boardRenderer = boardManager.GetComponent<BoardRenderer>();

            // Create dice controller
            // var diceController = Instantiate(diceControllerPrefab);
            var diceController = diceControllerPrefab;
            _diceRoller = diceController.GetComponent<DiceController>();

            // Create token manager
            // tokenManager = Instantiate(tokenManagerPrefab);
            _tokenManager = tokenManagerPrefab;

            // Create UI manager
            // uiManager = Instantiate(uiManagerPrefab);
            _uiManager = uiManagerPrefab;

            // Initialize timer and audio systems
            _timerSystem = GetComponent<TimerSystem>();
            _audioSystem = GetComponent<AudioSystem>();

            // Create players (1 human + 3 AI)
            CreatePlayers();

            // Start game with a random player or human player
            _currentPlayerIndex = Random.Range(0, 1) == 0 ? humanPlayerIndex : Random.Range(0, 4);
            StartTurn();
        }

        private void CreatePlayers()
        {
            // Create the human player
            var humanPlayer = new HumanPlayer(TokenColor.Yellow, this, _gameRules);
            _players.Add(humanPlayer);

            // Create different AI players with different strategies
            var easyAI = new AIPlayer(TokenColor.Green, this, _gameRules, new EasyAIStrategy());
            var mediumAI = new AIPlayer(TokenColor.Red, this, _gameRules, new MediumAIStrategy());
            var hardAI = new AIPlayer(TokenColor.Blue, this, _gameRules, new HardAIStrategy());

            _players.Add(easyAI);
            _players.Add(mediumAI);
            _players.Add(hardAI);

            // Initialize tokens for each player and create their visual representations
            foreach (var player in _players)
            {
                player.InitializeTokens();

                // Create token objects for visual representation
                foreach (var token in player.GetTokens())
                {
                    _tokenManager.CreateTokenObject(token);
                }
            }

            // Set up player relationships (who are the other players)
            foreach (var player in _players)
            {
                player.SetOtherPlayers(_players);
            }
        }

        private void StartTurn()
        {
            if (_gameOver) return;

            _currentState = GameState.WaitingForRoll;
            Player currentPlayer = _players[_currentPlayerIndex];

            _uiManager.UpdateTurnIndicator(currentPlayer.Color);

            if (currentPlayer is HumanPlayer)
            {
                // Enable dice rolling for human player
                _diceRoller.EnableRolling(true);

                // Start roll timer for human player
                _timerSystem.StartTimer(10f, () =>
                {
                    // Auto-roll if timer expires
                    RollDice();
                });
            }
            else
            {
                // AI player will roll after a delay
                StartCoroutine(AITurnDelay());
            }
        }

        private IEnumerator AITurnDelay()
        {
            // AI delay before rolling
            yield return new WaitForSeconds(2f);

            // Roll dice for AI
            RollDice();
        }

        public void RollDice()
        {
            if (_currentState != GameState.WaitingForRoll || _gameOver) return;

            // Stop any existing timer
            _timerSystem.StopTimer();

            // Roll the dice
            int diceValue = _diceRoller.Roll();
            _audioSystem.PlayDiceRoll();

            // Check if player rolled a 6
            if (diceValue == 6)
            {
                _consecutiveSixes++;

                // If player rolled 3 consecutive sixes
                if (_consecutiveSixes >= 3)
                {
                    // Skip turn after 3 consecutive sixes
                    _consecutiveSixes = 0;
                    EndTurn();
                    return;
                }
            }
            else
            {
                _consecutiveSixes = 0;
            }

            // Get valid moves for current player
            Player currentPlayer = _players[_currentPlayerIndex];
            List<Move> validMoves = _gameRules.GetValidMoves(currentPlayer, diceValue);

            if (validMoves.Count == 0)
            {
                // No valid moves, end turn
                EndTurn();
                return;
            }

            // Update game state
            _currentState = GameState.WaitingForMove;

            if (currentPlayer is HumanPlayer)
            {
                // Store valid moves and wait for human selection
                _currentValidMoves = validMoves;
                _waitingForHumanMoveSelection = true;

                // Show clickable highlights for human player
                _boardRenderer.HighlightValidMovesForHuman(validMoves, this);

                // Start move timer
                _timerSystem.StartTimer(15f, () =>
                {
                    // Auto-move if timer expires (choose first valid move)
                    OnMoveSelected(validMoves[0]);
                });
            }
            else
            {
                // AI decides and makes a move after delay
                StartCoroutine(AIMoveDelay(validMoves));
            }
        }

        // New method called when human player clicks on a highlight
        public void OnMoveSelected(Move selectedMove)
        {
            if (!_waitingForHumanMoveSelection || _currentState != GameState.WaitingForMove)
                return;

            // Verify the move is valid
            if (!_currentValidMoves.Contains(selectedMove))
            {
                Debug.LogWarning("Invalid move selected!");
                return;
            }

            // Reset selection state
            _waitingForHumanMoveSelection = false;
            _currentValidMoves.Clear();

            // Make the selected move
            MakeMove(selectedMove);
        }

        private IEnumerator AIMoveDelay(List<Move> validMoves)
        {
            // Show regular highlights for AI (non-clickable)
            List<int> validPositions = new List<int>();
            foreach (var move in validMoves)
            {
                validPositions.Add(move.TargetPosition);
            }

            _boardRenderer.HighlightValidMoves(validPositions);

            // AI delay before moving
            yield return new WaitForSeconds(3f);

            // AI decides which move to make
            Player currentPlayer = _players[_currentPlayerIndex];
            Move selectedMove = currentPlayer.DecideMove(validMoves);

            // Make the selected move
            MakeMove(selectedMove);
        }

        public void MakeMove(Move move)
        {
            if (_currentState != GameState.WaitingForMove || _gameOver) return;

            // Stop any existing timer
            _timerSystem.StopTimer();

            // Clear highlights
            _boardRenderer.ClearHighlights();

            // Reset human selection state
            _waitingForHumanMoveSelection = false;
            _currentValidMoves.Clear();

            // Check if this move kills another token
            if (move.WillKill)
            {
                // Find and send home any tokens at the target position
                foreach (var player in _players)
                {
                    player.SendTokensHomeAt(move.TargetPosition, move.Token.Color);
                }

                _audioSystem.PlayTokenKill();
            }
            else
            {
                _audioSystem.PlayTokenMove();
            }

            // Move the token
            move.Token.CurrentPosition = move.TargetPosition;
            move.Token.IsInHome = false;
            move.Token.IsInPlay = true;

            // Check if token reached the final home position
            if (_gameRules.HasTokenReachedEnd(move.Token))
            {
                move.Token.HasReachedEnd = true;
                move.Token.IsInPlay = false;
            }

            // Update token position on the board
            _boardRenderer.UpdateTokenPosition(move.Token,
                _gameRules.GetPositionCoordinates(move.TargetPosition, move.Token.Color));

            // Check for victory
            Player currentPlayer = _players[_currentPlayerIndex];
            if (currentPlayer.HasWon())
            {
                _gameOver = true;

                // Show victory screen
                bool playerWon = currentPlayer is HumanPlayer;
                _uiManager.ShowVictoryScreen(playerWon);
                _audioSystem.PlayVictory();
                return;
            }

            // Continue turn if rolled a 6, otherwise end turn
            if (_diceRoller.GetLastRoll() == 6 && _consecutiveSixes < 3)
            {
                StartTurn();
            }
            else
            {
                EndTurn();
            }
        }

        private void EndTurn()
        {
            // Clear any remaining highlights and selection state
            _boardRenderer.ClearHighlights();
            _waitingForHumanMoveSelection = false;
            _currentValidMoves.Clear();

            // Move to next player
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            StartTurn();
        }

        public void RestartGame()
        {
            // Reset game state
            _gameOver = false;
            _currentPlayerIndex = Random.Range(0, 1) == 0 ? humanPlayerIndex : Random.Range(0, 4);
            _consecutiveSixes = 0;
            _waitingForHumanMoveSelection = false;
            _currentValidMoves.Clear();

            // Clear highlights
            _boardRenderer.ClearHighlights();

            // Reset players and tokens
            foreach (var player in _players)
            {
                player.ResetTokens();

                // Reset visual token positions
                foreach (var token in player.GetTokens())
                {
                    Vector2 homePosition = _tokenManager.GetComponent<BoardManager>()
                        .GetHomeCoordinates(token.Color, token.TokenIndex);
                    _boardRenderer.UpdateTokenPosition(token, homePosition);
                }
            }

            // Reset UI
            _uiManager.HideVictoryScreen();

            // Start new game
            StartTurn();
        }
    }
}