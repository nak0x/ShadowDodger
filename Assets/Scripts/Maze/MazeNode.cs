using System.Collections.Generic;
using Chunks;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

namespace Maze
{
    /// <summary>
    /// Represents a node in the maze, which can be connected to other nodes and has walls that can be carved.
    /// This class handles the maze node's position, direction, and wall states.
    /// Walls are represented as a Vector4, where each component corresponds to a wall state:
    /// - x: Left wall (1 = active, 0 = inactive)
    /// - y: Up wall (1 = active, 0 = inactive)
    /// - z: Right wall (1 = active, 0 = inactive)
    /// - w: Down wall (1 = active, 0 = inactive)
    /// The node can instantiate a chunk prefab that represents the cell in the maze.
    /// The chunk prefab must have a `ChunkCell` component to manage the walls states.
    /// </summary>
    public class MazeNode : MonoBehaviour
    {
        [Header("Maze Node Settings")]
        [Tooltip("Prefab for the node chunk that represents this maze node. The prefab must have a ChunkCell component.")]
        [SerializeField] private GameObject nodeChunkPrefab;

        [Tooltip("The identity vector of the node, which includes its position and direction in the maze.")]
        /// <summary>
        /// The identity vector of the node, which includes its position and direction in the maze.
        /// The y component represents the direction (0, 90, 180, 270 degrees or -1 for no direction).
        /// The x and z components represent the position in the maze.
        /// </summary>
        [SerializeField] private Vector3 _identity = new Vector3(0, -1, 0);

        [Header("Debugging Settings")]
        [Tooltip("Whether to draw gizmos for this node in the editor.")]
        [SerializeField] private bool _drawGizmos = false;

        /// <summary>
        /// Unique identifier for the node in the format "x_z", where x and z are the maze coordinates.
        /// This ID is used to identify the node in the maze and can be set based on its position.
        /// </summary>
        private string _id { get; set; }

        /// <summary>
        /// Instance of the node chunk prefab that represents this maze node.
        /// This instance is created when the node is rendered and contains the walls of the maze.
        /// </summary>
        private GameObject _nodeChunkInstance;

        /// <summary>
        /// Dictionary mapping direction angles to wall states.
        /// The keys are angles in degrees (0, 90, 180, 270) or -1 for no direction.
        /// The values are Vector4s representing the wall states for each direction:
        /// - Up (90): (0, 1, 1, 1)
        /// - Right (0): (1, 0, 1, 1)
        /// - Down (270): (1, 1, 0, 1)
        /// - Left (180): (1, 1, 1, 0)
        /// - No direction (-1): (1, 1, 1, 1) - all walls active
        /// This dictionary is used to determine which walls to carve based on the node's direction.
        /// Reference: ChunkCell class for wall states.
        /// </summary>
        private Dictionary<int, Vector4> _directionWall = new Dictionary<int, Vector4>
        {
            { 90, new Vector4(0,1,1,1) }, // Up
            { 0, new Vector4(1,0,1,1) }, // Right
            { 270, new Vector4(1,1,0,1) }, // Down
            { 180, new Vector4(1,1,1,0) }, // Left
            { -1, new Vector4(1,1,1,1) } // No direction
        };

        /// <summary>
        /// Initializes the maze node.
        /// This method checks if the node chunk prefab has a `ChunkCell` component,
        /// which is required for managing the walls of the maze node.
        /// </summary>
        public void Awake()
        {
            if (nodeChunkPrefab.GetComponent<ChunkCell>() == null)
            {
                Debug.LogError($"Node {_id}: Chunk prefab must have a ChunkCell component.");
                return;
            }

            _identity = new Vector3(transform.position.x, -1, transform.position.z);
        }

        public Vector3 GetMazePosition()
        {
            return new Vector3(_identity.x, 0, _identity.y);
        }

        /// <remarks>
        /// Sets the maze position of the node.
        /// This also set the unique identifier `_id` based on the new position.
        /// </remarks>
        public void SetMazePosition(int x, int y)
        {
            _identity.x = x;
            _identity.y = y;
            _id = $"{x}_{y}";
        }

        /// <summary>
        /// Gets the direction angle of the node.
        /// </summary>
        /// <returns>Direction angle (-1, 0, 90, 180, 270)</returns>
        public int GetNodeDirection()
        {
            return (int)_identity.z;
        }

        /// <summary>
        /// / Sets the direction of the node.
        /// The direction must be one of the following values: -1 (no direction), 0 (right), 90 (up), 180 (left), or 270 (down).
        /// If an invalid direction is provided, an error message is logged.
        /// The direction is stored in the z component of the identity vector.
        /// </summary>
        /// <param name="direction">A new facing direction for this node.</param>
        public void SetNodeDirection(float direction)
        {
            if (direction != -1 && direction != 0 && direction != 90 && direction != 180 && direction != 270)
            {
                Debug.LogError("Invalid direction value. Must be -1, 0, 90, 180, or 270 degrees.");
                return;
            }

            _identity.z = direction;
        }


        /// <summary>
        /// Ensures that the node chunk instance is created.
        /// If the instance does not exist, it instantiates a new one using the `nodeChunkPrefab`.
        /// The instance is positioned at the node's position and parented to the node's transform.
        /// </summary>
        public void EnsureNodeChunkInstance()
        {
            if (_nodeChunkInstance == null)
            {
                _nodeChunkInstance = Instantiate(nodeChunkPrefab, transform.position, Quaternion.identity, transform);
            }
        }
        /// <summary>
        /// Renders the maze node by ensuring the node chunk instance is created and carving the wall
        /// based on the node's direction.
        /// This method also opens the targetting node towards the current node's position.
        /// The targetting node is expected to be a neighboring node in the maze.
        /// The direction of the wall to carve is determined by the `_directionWall` dictionary,
        /// which maps the direction angle to the corresponding wall state.
        /// </summary>
        /// <param name="targettingNode">The node pointed by current's direction.</param>
        public void Render(MazeNode targettingNode)
        {
            EnsureNodeChunkInstance();
            CarveWall(_directionWall[(int)_identity.z]);
            targettingNode.OpenTowards(_identity.x, _identity.y);
        }

        /// <summary>
        /// Opens the walls of the current node towards the specified base matrices.
        /// This method determines which wall to open based on the difference between the current node's position
        /// and the specified base matrices.
        /// </summary>
        /// <param name="targetMatricesX">Target node X matric position</param>
        /// <param name="targetMatricesY">Target node Y matric position</param>
        public void OpenTowards(float targetMatricesX, float targetMatricesY)
        {
            EnsureNodeChunkInstance();
            Vector4 wallStates; // Default all walls to active

            int hAxis = (int)(targetMatricesX - _identity.x);
            int vAxis = (int)(targetMatricesY - _identity.y);

            if (vAxis == 1) // Up
            {
                wallStates = new Vector4(0, 1, 1, 1); // Open top wall
            }
            else if (hAxis == 1) // Right
            {
                wallStates = new Vector4(1, 0, 1, 1); // Open right wall
            }
            else if (vAxis == -1) // Down
            {
                wallStates = new Vector4(1, 1, 0, 1); // Open bottom wall
            }
            else if (hAxis == -1) // Left
            {
                wallStates = new Vector4(1, 1, 1, 0); // Open left wall
            }
            else
            {
                // Occure when the target position is the same as the current position
                // So prevent error on origin cell
                return;
            }

            CarveWall(wallStates);
        }

        /// <summary>
        /// Carves the walls of the current node based on the provided wall states.
        /// The wall states are represented as a Vector4, where each component corresponds to a wall state:
        /// - x: Left wall (1 = active, 0 = inactive)
        /// - y: Up wall (1 = active, 0 = inactive)
        /// - z: Right wall (1 = active, 0 = inactive)
        /// - w: Down wall (1 = active, 0 = inactive)
        /// This method updates the walls of the node chunk instance by calling the `Carve` method of the `ChunkCell` component.
        /// </summary>
        /// <param name="wallStates">Vector4 representing the walls state (0: No wall, 1: Wall)</param>
        /// <exception cref="System.NullReferenceException">Throw an exception if the node chunk does not have a ChunkCell component</exception>
        public void CarveWall(Vector4 wallStates)
        {
            EnsureNodeChunkInstance();
            ChunkCell cell = _nodeChunkInstance.GetComponent<ChunkCell>() ?? throw new System.NullReferenceException("Node chunk instance does not have a ChunkCell component.");
            cell.Carve(wallStates);
        }

        /// <summary>
        /// Resets the walls of the current node to their default state.
        /// This method sets all walls to active (1) by creating a new Vector4 with all components set to 1.
        /// It updates the walls of the node chunk instance by calling the `UpdateWalls` method of the `ChunkCell` component.
        /// This is useful for resetting the maze node to its initial state before any walls have been carved.
        /// </summary>
        /// <exception cref="System.NullReferenceException">Throw an exception if the node chunk does not have a ChunkCell component</exception>
        public void ResetWalls()
        {
            EnsureNodeChunkInstance();
            ChunkCell cell = _nodeChunkInstance.GetComponent<ChunkCell>() ?? throw new System.NullReferenceException("Node chunk instance does not have a ChunkCell component.");
            cell.activeWalls = new Vector4(1, 1, 1, 1); // Reset all walls to active
            cell.UpdateWalls();
        }

        /// <summary>
        /// Resets the content of the current node chunk instance.
        /// This method calls the `ResetContent` method of the `ChunkCell` component,
        /// which is responsible for clearing any content that has been spawned in the chunk.
        /// </summary>
        /// <exception cref="System.NullReferenceException">Raise an exception if the chunkInstance dont contain a ChunkCell component</exception>
        public void RenewContent()
        {
            ChunkCell cell = _nodeChunkInstance.GetComponent<ChunkCell>() ?? throw new System.NullReferenceException("Node chunk instance does not have a ChunkCell component.");
            cell.RenewContent();
        }

        /// <summary>
        /// Draws gizmos in the editor to visualize the maze node.
        /// The gizmos include a sphere at the node's position to indicate its location,
        /// and a second sphere at the facing position if the node has a valid direction.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (!_drawGizmos) return;
            Gizmos.color = Color.blue;
            if (GetNodeDirection() < 0)
                Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.2f);

            if (GetNodeDirection() >= 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(GetFacingPosition(), 0.2f);
            }
            Gizmos.color = Color.white;
            Handles.Label(transform.position + new Vector3(0.5f, 0, 0.5f), _id);
        }

        /// <summary>
        /// Calculates the facing position of the node based on its direction.
        /// The facing position is determined by the angle of the node's direction,
        /// which is stored in the z component of the identity vector.
        /// Used only for visualization in the editor.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetFacingPosition()
        {
            float angleRad = _identity.z * Mathf.Deg2Rad;
            return new Vector3(transform.position.x + Mathf.Cos(angleRad), 0, transform.position.z + Mathf.Sin(angleRad));
        }
    }
}