using UnityEngine;

namespace Chunks
{
    public class ChunkCell : MonoBehaviour
    {
        [Header("Chunk Manager")]
        [SerializeField] private GameObject[] walls; // Array of chunk prefabs

        [SerializeField] public Vector4 activeWalls = new Vector4(1, 1, 1, 1); // Active walls in the order: left, right, top, bottom

        public void UpdateWalls()
        {
            // Instantiate the walls based on the activeWalls vector
            walls[0].SetActive(activeWalls.x == 1); // Left wall
            walls[1].SetActive(activeWalls.y == 1); // Right wall
            walls[2].SetActive(activeWalls.z == 1); // Top wall
            walls[3].SetActive(activeWalls.w == 1); // Bottom wall
        }

        public void Carve(Vector4 wallStates)
        {
            activeWalls = new Vector4(
                Mathf.Min(activeWalls.x, wallStates.x),
                Mathf.Min(activeWalls.y, wallStates.y),
                Mathf.Min(activeWalls.z, wallStates.z),
                Mathf.Min(activeWalls.w, wallStates.w)
            );
            UpdateWalls();
        }
    }
}