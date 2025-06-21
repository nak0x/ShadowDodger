using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        [Header("Maze Generation Settings")]
        [SerializeField] public int MazeWidth = 10;
        [SerializeField] public int MazeHeight = 10;
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private int cellSize = 5;
        [SerializeField] private Vector2Int origin = new Vector2Int(0, 0);

        [Header("Runtime shuffle Settings")]
        [Tooltip("Time interval between jumps in seconds. Set to 0 for immediate jumps.")]
        [SerializeField] private float intervalSeconds = 2f;
        [SerializeField] private bool enableRuntimeShuffle = true;
        [SerializeField] private int runtimeShuffleSteps = 20;

        private Dictionary<string, int> nodeDirections = new Dictionary<string, int>
        {
            { "down", 270 },
            { "right", 180 },
            { "up", 90 },
            { "left", 0 }
        };

        private Dictionary<int, int> nodeAngleMatriceModificator = new Dictionary<int, int>
        {
            { -1, 0},
            { 0, 1},
            { 90, 1},
            { 180, -1},
            { 270, -1}
        };

        private string[] _nodeDirectionsList = new string[] { "down", "right", "up", "left" };
        private float timer;

        private MazeNode[,] mazeNodes;

        void Awake()
        {
            SpawnNodes();
            InitialShuffle();
            RenderMaze();
        }

        void Update()
        {
            if (enableRuntimeShuffle)
            {
                timer += Time.deltaTime;
                if (timer >= intervalSeconds)
                {
                    timer -= intervalSeconds; // preserves leftover time
                    RuntimeShuffle();
                    RenderMaze();
                }
            }
        }

        void RenderMaze()
        {
            for (int x = 0; x < MazeWidth; x++)
            {
                for (int z = 0; z < MazeHeight; z++)
                {
                    if (mazeNodes[x, z] != null)
                    {
                        mazeNodes[x, z].Render(GetTargettingNode(x, z));
                    }
                }
            }
        }

        private MazeNode GetTargettingNode(int x, int z)
        {
            int angle = (int)mazeNodes[x, z]?.GetNodeDirection();
            if (angle == 0 || angle == 180)
            {
                return mazeNodes[x + nodeAngleMatriceModificator[angle], z] ?? throw new System.Exception($"MazeNode at ({x + nodeAngleMatriceModificator[angle]}, {z}) is null. Ensure nodes are spawned correctly.");
            }
            else if (angle == 90 || angle == 270)
            {
                return mazeNodes[x, z + nodeAngleMatriceModificator[angle]] ?? throw new System.Exception($"MazeNode at ({x}, {z + nodeAngleMatriceModificator[angle]}) is null. Ensure nodes are spawned correctly.");
            }
            return mazeNodes[x, z] ?? throw new System.Exception($"MazeNode at ({x}, {z}) is null. Ensure nodes are spawned correctly.");
        }

        public void SpawnNodes()
        {
            mazeNodes = new MazeNode[MazeWidth, MazeHeight];

            for (int x = 0; x < MazeWidth; x++)
            {
                for (int z = 0; z < MazeHeight; z++)
                {
                    Vector3 position = new Vector3(cellSize * x, 0, cellSize * z);
                    GameObject nodeObject = Instantiate(nodePrefab, position, Quaternion.identity, transform);
                    MazeNode node = nodeObject.GetComponent<MazeNode>();
                    if (node != null)
                    {
                        mazeNodes[x, z] = node;
                        node.SetMazePosition(x, z);
                    }
                }
            }

            InitializeNodeOrientation();
        }

        public void InitializeNodeOrientation()
        {
            List<Vector2Int> spiralPath = new List<Vector2Int>();

            int top = 0, bottom = MazeHeight - 1;
            int left = 0, right = MazeWidth - 1;

            while (left <= right && top <= bottom)
            {
                for (int z = top; z <= bottom; z++)
                    spiralPath.Add(new Vector2Int(left, z));
                left++;

                for (int x = left; x <= right; x++)
                    spiralPath.Add(new Vector2Int(x, bottom));
                bottom--;

                for (int z = bottom; z >= top; z--)
                    spiralPath.Add(new Vector2Int(right, z));
                right--;

                for (int x = right; x >= left; x--)
                    spiralPath.Add(new Vector2Int(x, top));
                top++;
            }

            // Set each node to point toward the next node
            for (int i = 0; i < spiralPath.Count - 1; i++)
            {
                Vector2Int current = spiralPath[i];
                Vector2Int next = spiralPath[i + 1];

                Vector3 dir = mazeNodes[next.x, next.y].GetPosition() - mazeNodes[current.x, current.y].GetPosition();
                float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

                // Normalize to 0, 90, 180, 270
                angle = Mathf.Round(angle / 90f) * 90f;
                if (angle < 0) angle += 360f;
                mazeNodes[current.x, current.y].SetNodeDirection(angle);
            }

            // Optionally set the last node's direction to -1 (no direction)
            Vector2Int last = spiralPath[spiralPath.Count - 1];
            mazeNodes[last.x, last.y].SetNodeDirection(-1);
            SetOrigin(last.x, last.y);
        }

        public void SetOrigin(int x, int z)
        {
            origin = new Vector2Int(x, z);
        }

        public void FindOrigin()
        {
            for (int x = 0; x < MazeWidth; x++)
            {
                for (int z = 0; z < MazeHeight; z++)
                {
                    if (mazeNodes[x, z].GetNodeDirection() < 0)
                    {
                        SetOrigin(x, z);
                        return;
                    }
                }
            }
            Debug.LogWarning("No origin found in the maze.");
        }

        string GetAvailableRandomDirection()
        {
            List<string> directions = new();

            if (origin.x > 0) directions.Add("right");
            if (origin.y > 0) directions.Add("down");
            if (origin.x < MazeWidth - 1) directions.Add("left");
            if (origin.y < MazeHeight - 1) directions.Add("up");

            return directions[Random.Range(0, directions.Count)];
        }


        void PerformOriginJump(OriginJumpMode mode = OriginJumpMode.Subtract)
        {
            // Get a random jump direction
            int angle = nodeDirections[GetAvailableRandomDirection()];

            // Set the direction of the current node to the jump direction
            mazeNodes[origin.x, origin.y].SetNodeDirection(angle);

            // Move the origin in the specified direction
            if (angle == 0 || angle == 180)
            {
                origin.x = Mathf.Clamp(origin.x + nodeAngleMatriceModificator[angle], 0, MazeWidth - 1);
            }
            else if (angle == 90 || angle == 270)
            {
                origin.y = Mathf.Clamp(origin.y + nodeAngleMatriceModificator[angle], 0, MazeHeight - 1);
            }

            // Update the origin node's direction
            mazeNodes[origin.x, origin.y].SetNodeDirection(-1);
        }

        public void InitialShuffle()
        {
            int initialShuffleCount = MazeWidth * MazeHeight * 10;
            for (int i = 0; i < initialShuffleCount; i++)
            {
                PerformOriginJump();
            }
        }

        public void RuntimeShuffle()
        {
            for (int x = 0; x < MazeWidth; x++)
            {
                for (int z = 0; z < MazeHeight; z++)
                {
                    if (mazeNodes[x, z] != null)
                    {
                        mazeNodes[x, z].ResetWalls(); // Reset direction to -1
                    }
                }
            }

            int runtimeShuffleCount = MazeWidth * MazeHeight * runtimeShuffleSteps;
            for (int i = 0; i < runtimeShuffleCount; i++)
            {
                PerformOriginJump();
            }
        }

    }

    enum OriginJumpMode
    {
        Subtract,
        Add,
        Toggle
    }
}