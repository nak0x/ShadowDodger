using UnityEngine;
using Utils;
using EditorAttributes;

namespace Maze
{
    public class MazeManager : MonoBehaviour
    {
        [Header("Maze Generation Settings")]
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private Seed _seed;

        void Awake()
        {
            if (_seed == null)
                _seed = new Seed();

            _seed.InitIfUnset();
            mazeGenerator.GenerateMaze(_seed);
        } 

        [ContextMenu("Regenerate Maze")]
        [Button("Regenerate Maze", 30f)]
        public void RegenerateMaze()
        {
            mazeGenerator.RegenerateMaze();
        }
    }
}