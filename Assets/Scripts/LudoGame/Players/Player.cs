using System.Collections.Generic;
using UnityEngine;
using LudoGame.Tokens;
using LudoGame.Rules;
using LudoGame.Core;

namespace LudoGame.Players
{
    /// <summary>
    /// Abstract base class for all player types
    /// </summary>
    public abstract class Player
    {
        public TokenColor Color { get; private set; }
        protected List<Token> Tokens = new List<Token>();
        protected GameManager GameManager;
        protected IGameRules GameRules;
        protected List<Player> OtherPlayers = new List<Player>();

        public Player(TokenColor color, GameManager manager, IGameRules rules)
        {
            Color = color;
            GameManager = manager;
            GameRules = rules;
        }

        public void InitializeTokens()
        {
            // Create 4 tokens for this player
            for (int i = 0; i < 4; i++)
            {
                Token token = new Token(Color, i);
                Tokens.Add(token);
            }
        }

        public void SetOtherPlayers(List<Player> players)
        {
            OtherPlayers = new List<Player>();
            foreach (var player in players)
            {
                if (player != this)
                {
                    OtherPlayers.Add(player);
                }
            }
        }

        public List<Player> GetOtherPlayers()
        {
            return OtherPlayers;
        }

        public List<Token> GetTokens()
        {
            return Tokens;
        }

        public bool HasSingleTokenAt(int position)
        {
            int count = 0;
            foreach (Token token in Tokens)
            {
                if (token.CurrentPosition == position && token.IsInPlay)
                {
                    count++;
                }
            }

            return count == 1;
        }

        public void SendTokensHomeAt(int position, TokenColor attackerColor)
        {
            // If this is not a safe position and we have a single token here, send it home
            if (!GameRules.IsSafePosition(position, Color) && HasSingleTokenAt(position) && Color != attackerColor)
            {
                foreach (Token token in Tokens)
                {
                    if (token.CurrentPosition == position && token.IsInPlay)
                    {
                        token.Reset();
                        break;
                    }
                }
            }
        }

        public bool HasWon()
        {
            // Player wins when all 4 tokens have reached the end
            foreach (Token token in Tokens)
            {
                if (!token.HasReachedEnd)
                {
                    return false;
                }
            }

            return true;
        }

        public void ResetTokens()
        {
            foreach (Token token in Tokens)
            {
                token.Reset();
            }
        }

        public abstract Move DecideMove(List<Move> validMoves);
        public abstract bool ShouldRoll();
    }
}
