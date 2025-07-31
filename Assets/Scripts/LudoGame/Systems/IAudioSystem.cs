namespace LudoGame.Systems
{
    /// <summary>
    /// Interface for game audio functionality
    /// </summary>
    public interface IAudioSystem
    {
        void PlayDiceRoll();
        void PlayTokenMove();
        void PlayTokenKill();
        void PlayVictory();
    }
}
