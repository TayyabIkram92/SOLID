using LudoGame.Tokens;

namespace LudoGame.Rules
{
    /// <summary>
    /// Represents a possible move for a token
    /// </summary>
    public class Move
    {
        public Token Token { get; set; }
        public int Steps { get; set; }
        public int TargetPosition { get; set; }
        public bool WillKill { get; set; }

        public Move(Token token, int steps, int targetPosition, bool willKill)
        {
            Token = token;
            Steps = steps;
            TargetPosition = targetPosition;
            WillKill = willKill;
        }
    }
}
