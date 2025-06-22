using UnityEngine;

namespace Chunks
{
    /// <summary>
    /// ChunkCell is a component that manages the walls of a chunk in a grid-based system.
    /// It allows for dynamic updates of wall states and can carve out walls based on specified conditions.
    /// </summary>
    public class ChunkCell : MonoBehaviour
    {
        [Header("Chunk Cell Content")]
        [SerializeField] private ChunkContentManager content; // Prefab for the content of the

        [Header("Wall Objects")]
        [SerializeField] private GameObject topWalls; // Array of chunk prefabs
        [SerializeField] private GameObject rightWalls; // Array of chunk prefabs
        [SerializeField] private GameObject bottomWalls; // Array of chunk prefabs
        [SerializeField] private GameObject leftWalls; // Array of chunk prefabs

        [Header("Active Walls")]
        /// <summary>
        /// Vector4 representing the active walls of the chunk.
        /// Each component corresponds to a wall: top, right, bottom, left.
        /// A value of 1 means the wall is active (visible), and 0 means it is inactive (hidden).
        /// </summary>
        [SerializeField] public Vector4 activeWalls = new Vector4(1, 1, 1, 1);

        /// <summary>
        /// Updates the visibility of the walls based on the activeWalls vector.
        /// This method should be called whenever the activeWalls vector is modified to reflect the current state of the chunk.
        /// </summary>
        public void UpdateWalls()
        {
            // Instantiate the walls based on the activeWalls vector
            topWalls.SetActive(activeWalls.x == 1); // Top wall
            rightWalls.SetActive(activeWalls.y == 1); // Right wall
            bottomWalls.SetActive(activeWalls.z == 1); // Bottom wall
            leftWalls.SetActive(activeWalls.w == 1); // Left wall
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

        public void RenewContent()
        {
            content.RenewContent();
        }
    }
}