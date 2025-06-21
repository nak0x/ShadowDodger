using UnityEngine;
using Chunks;

public class ChunkSpawner : MonoBehaviour
{
    public GameObject chunkPrefab;
    public int gridSize = 5; // n x n grid
    public float chunkSize = 5f; // distance between chunks

    void Start()
    {
        GenerateChunks();
    }

    void GenerateChunks()
    {
        Vector3 start = transform.position;
        float halfSize = (gridSize - 1) / 2f;

        for (int i = 0; i < gridSize * gridSize; i++)
        {
            int x = i % gridSize;
            int z = i / gridSize;

            float posX = (x - halfSize) * chunkSize;
            float posZ = (z - halfSize) * chunkSize;

            Vector3 pos = new Vector3(posX, 0, posZ) + start;
            GameObject chunk = Instantiate(chunkPrefab, pos, Quaternion.identity, transform);

            ChunkManager manager = chunk.GetComponent<ChunkManager>();
            if (manager != null)
            {
                manager.activeWalls = new Vector4(
                    x > 0 ? 1 : 0,
                    x < gridSize - 1 ? 1 : 0,
                    z > 0 ? 1 : 0,
                    z < gridSize - 1 ? 1 : 0
                );
            }

            chunk.name = $"Chunk_{x}_{z}";
        }
    }


}

