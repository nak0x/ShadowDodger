using UnityEngine;

namespace Maze
{
    public class MazeManager : MonoBehaviour
    {
        [Header("Maze Generation Settings")]
        // [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private int _seed = 0;

        void Awake()
        {
            // mazeGenerator.GenerateMaze(_seed);
        } 
    }
}