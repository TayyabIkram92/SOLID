namespace LudoGame.Dice
{
    /// <summary>
    /// Interface for dice rolling functionality
    /// </summary>
    public interface IDiceRoller
    {
        int Roll();
        int GetLastRoll();
        void EnableRolling(bool enabled);
    }
}
