using UnityEngine;

namespace Chunks
{
    public class ChunkManager : MonoBehaviour
    {
        [Header("Chunk Manager")]
        [SerializeField] private GameObject[] walls; // Array of chunk prefabs

        [SerializeField] public Vector4 activeWalls = new Vector4(1, 1, 1, 1); // Active walls in the order: left, right, top, bottom

        void Awake()
        {
            // Instantiate the walls based on the activeWalls vector
            walls[0].SetActive(activeWalls.x == 1); // Left wall
            walls[1].SetActive(activeWalls.y == 1); // Right wall
            walls[2].SetActive(activeWalls.z == 1); // Top wall
            walls[3].SetActive(activeWalls.w == 1); // Bottom wall
        }

        public void toggleWall(Vector4 wallStates)
        {
            // Toggle the walls based on the provided wallStates vector
            walls[0].SetActive(wallStates.x == 1); // Left wall
            walls[1].SetActive(wallStates.y == 1); // Right wall
            walls[2].SetActive(wallStates.z == 1); // Top wall
            walls[3].SetActive(wallStates.w == 1); // Bottom wall
        }
    }
}