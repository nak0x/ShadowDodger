using UnityEngine;
using Chunks;
using System.Collections.Generic;

namespace Level
{
    public class MapManager : MonoBehaviour
    {
        public GameObject chunkPrefab;
        public int baseSize = 5;
        public float chunkSize = 5f;

        private ChunkManager[,] grid;
        private Vector4[,] wallStates; // Store wall state for each chunk
        private int currentLevel;
        private System.Random rng;

        public void GenerateLevel(int level, int? seed = null)
        {
            currentLevel = level;
            int gridSize = baseSize + level;

            int usedSeed = seed ?? UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            rng = new System.Random(usedSeed);

            Debug.Log($"Generating level {level} with seed: {usedSeed}");

            ClearPreviousChunks();
            GenerateGrid(gridSize);
            GenerateMaze(gridSize);
            ClampOuterWalls(gridSize);
            ApplyWalls(gridSize);
        }

        void ClampOuterWalls(int gridSize)
        {
            for (int x = 0; x < gridSize; x++)
            {
                wallStates[x, 0].z = 1; // bottom row can't open -Z
                wallStates[x, gridSize - 1].w = 1; // top row can't open +Z
            }
            for (int z = 0; z < gridSize; z++)
            {
                wallStates[0, z].x = 1; // left column can't open -X
                wallStates[gridSize - 1, z].y = 1; // right column can't open +X
            }
        }


        void ClearPreviousChunks()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        void GenerateGrid(int gridSize)
        {
            grid = new ChunkManager[gridSize, gridSize];
            wallStates = new Vector4[gridSize, gridSize]; // Init walls

            float half = (gridSize - 1) / 2f;
            Vector3 origin = transform.position;

            for (int i = 0; i < gridSize * gridSize; i++)
            {
                int x = i % gridSize;
                int z = i / gridSize;

                Vector3 pos = new Vector3((x - half) * chunkSize, 0, (z - half) * chunkSize) + origin;
                GameObject chunk = Instantiate(chunkPrefab, pos, Quaternion.identity, transform);

                ChunkManager manager = chunk.GetComponent<ChunkManager>();
                grid[x, z] = manager;

                wallStates[x, z] = Vector4.one; // all walls active initially
            }
        }

        void GenerateMaze(int gridSize)
        {
            bool[,] visited = new bool[gridSize, gridSize];
            Visit(0, 0, visited, gridSize);
        }

        void Visit(int x, int z, bool[,] visited, int gridSize)
        {
            visited[x, z] = true;

            var directions = new List<Vector2Int> {
                Vector2Int.right, Vector2Int.left,
                Vector2Int.up, Vector2Int.down
            };
            Shuffle(directions);

            foreach (var dir in directions)
            {
                int nx = x + dir.x;
                int nz = z + dir.y;

                if (nx >= 0 && nx < gridSize && nz >= 0 && nz < gridSize && !visited[nx, nz])
                {
                    RemoveWallBetween(x, z, nx, nz);
                    Visit(nx, nz, visited, gridSize);
                }
            }
        }

        void RemoveWallBetween(int x1, int z1, int x2, int z2)
        {
            int dx = x2 - x1;
            int dz = z2 - z1;

            // Modify stored wall state instead of calling toggleWall now
            if (dx == 1)      { wallStates[x1, z1].y = 0; wallStates[x2, z2].x = 0; } // right
            else if (dx == -1){ wallStates[x1, z1].x = 0; wallStates[x2, z2].y = 0; } // left
            else if (dz == 1) { wallStates[x1, z1].w = 0; wallStates[x2, z2].z = 0; } // up
            else if (dz == -1){ wallStates[x1, z1].z = 0; wallStates[x2, z2].w = 0; } // down
        }

        void ApplyWalls(int gridSize)
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    grid[x, z].toggleWall(wallStates[x, z]);
                }
            }
        }

        void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int rand = rng.Next(i + 1);
                (list[i], list[rand]) = (list[rand], list[i]);
            }
        }
    }
}
