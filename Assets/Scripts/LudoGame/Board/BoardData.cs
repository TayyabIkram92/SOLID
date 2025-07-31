using UnityEngine;
using UnityEngine.Serialization;

namespace LudoGame.Board
{
    /// <summary>
    /// ScriptableObject containing board configuration data
    /// </summary>
    [CreateAssetMenu(fileName = "BoardData", menuName = "Ludo/Board Data")]
    public class BoardData : ScriptableObject
    {
        [FormerlySerializedAs("PositionCoordinates")]
        [Header("Board Layout")]
        [Tooltip("Coordinates for each position on the board (0-51 for main track)")]
        public Transform[] positionCoordinates = new Transform[52];

        [FormerlySerializedAs("RedStartPosition")] [Header("Starting Positions")]
        public int redStartPosition = 0;
        [FormerlySerializedAs("BlueStartPosition")] public int blueStartPosition = 13;
        [FormerlySerializedAs("GreenStartPosition")] public int greenStartPosition = 26;
        [FormerlySerializedAs("YellowStartPosition")] public int yellowStartPosition = 39;

        [FormerlySerializedAs("CommonSafePositions")]
        [Header("Safe Positions")]
        [Tooltip("Common safe positions accessible by all players")]
        public int[] commonSafePositions = new int[8] { 0, 8, 13, 21, 26, 34, 39, 47 };

        [FormerlySerializedAs("RedSafePositions")] [Tooltip("Safe positions specific to Red tokens")]
        public int[] redSafePositions = new int[5] { 100, 101, 102, 103, 104 };

        [FormerlySerializedAs("BlueSafePositions")] [Tooltip("Safe positions specific to Blue tokens")]
        public int[] blueSafePositions = new int[5] { 100, 101, 102, 103, 104 };

        [FormerlySerializedAs("GreenSafePositions")] [Tooltip("Safe positions specific to Green tokens")]
        public int[] greenSafePositions = new int[5] { 100, 101, 102, 103, 104 };

        [FormerlySerializedAs("YellowSafePositions")] [Tooltip("Safe positions specific to Yellow tokens")]
        public int[] yellowSafePositions = new int[5] { 100, 101, 102, 103, 104 };

        [Header("Home Positions")]
        [Tooltip("Initial home positions for each color's tokens")]
        public Vector2[][] HomePositions = new Vector2[4][];

        [Header("Final Home Positions")]
        [Tooltip("Final home positions for each color's tokens")]
        public Vector2[][] FinalHomePositions = new Vector2[4][];

        [Header("Home Stretch Positions")]
        [Tooltip("Position coordinates for final approach to home (specific to each color)")]
        public Vector2[][] HomeStretchPositions = new Vector2[4][];

        private void OnEnable()
        {
            // Initialize arrays if they haven't been set up yet
            for (int i = 0; i < 4; i++)
            {
                if (HomePositions[i] == null)
                    HomePositions[i] = new Vector2[4];

                if (FinalHomePositions[i] == null)
                    FinalHomePositions[i] = new Vector2[4];

                if (HomeStretchPositions[i] == null)
                    HomeStretchPositions[i] = new Vector2[5];
            }
        }
    }
}
