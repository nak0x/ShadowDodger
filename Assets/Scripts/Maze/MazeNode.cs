using System.Collections.Generic;
using Chunks;
using UnityEditor;
using UnityEngine;

namespace Maze
{
    public class MazeNode : MonoBehaviour
    {
        [Header("Maze Node Settings")]
        [SerializeField] private GameObject nodeChunkPrefab;

        [SerializeField] private Vector3 _identity = new Vector3(0, -1, 0);

        private string _id { get; set; }
        private GameObject _nodeChunkInstance;
        private Vector2 _mazePosition;

        private Dictionary<int, Vector4> _directionWall = new Dictionary<int, Vector4>
        {
            { 180, new Vector4(1,1,1,0) }, // Left
            { 90, new Vector4(0,1,1,1) }, // Up
            { 0, new Vector4(1,1,0,1) }, // Right
            { 270, new Vector4(1,0,1,1) }, // Down
            { -1, new Vector4(1,1,1,1) } // No direction
        };

        public void Awake()
        {
            if (nodeChunkPrefab.GetComponent<ChunkCell>() == null)
            {
                Debug.LogError($"Node {_id}: Chunk prefab must have a ChunkCell component.");
                return;
            }

            _identity = new Vector3(transform.position.x, -1, transform.position.z);
        }

        public void SetMazePosition(int x, int y)
        {
            _mazePosition = new Vector2(x, y);
            SetId(x, y);
        }

        public void SetId(int x, int z)
        {
            _id = $"{x}_{z}";
        }

        public float GetNodeDirection()
        {
            return _identity.y;
        }

        public void SetNodeDirection(float direction)
        {
            if (direction != -1 && direction != 0 && direction != 90 && direction != 180 && direction != 270)
            {
                Debug.LogError("Invalid direction value. Must be -1, 0, 90, 180, or 270 degrees.");
                return;
            }

            _identity.y = direction;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(_identity.x, 0, _identity.z);
        }

        public Vector3 GetFacingPosition()
        {
            float angleRad = _identity.y * Mathf.Deg2Rad;
            return new Vector3(_identity.x + Mathf.Cos(angleRad), 0, _identity.z + Mathf.Sin(angleRad));
        }

        public void ensureNodeChunkInstance()
        {
            if (_nodeChunkInstance == null)
            {
                _nodeChunkInstance = Instantiate(nodeChunkPrefab, GetPosition(), Quaternion.identity, transform);
            }
        }

        public void Render(MazeNode targettingNode)
        {
            ensureNodeChunkInstance();
            CarveWall(_directionWall[(int)_identity.y]);
            targettingNode.OpenTowards(_mazePosition);
        }

        public void OpenTowards(Vector2 targetPosition)
        {
            ensureNodeChunkInstance();
            Vector4 wallStates; // Default all walls to active

            int hAxis = (int)(targetPosition.x - _mazePosition.x);
            int vAxis = (int)(targetPosition.y - _mazePosition.y);

            if (hAxis == 1) // Right
            {
                wallStates = new Vector4(1, 1, 0, 1); // Open right wall
            }
            else if (hAxis == -1) // Left
            {
                wallStates = new Vector4(1, 1, 1, 0); // Open left wall
            }
            else if (vAxis == 1) // Up
            {
                wallStates = new Vector4(0, 1, 1, 1); // Open top wall
            }
            else if (vAxis == -1) // Down
            {
                wallStates = new Vector4(1, 0, 1, 1); // Open bottom wall
            }
            else
            {
                // Occure when the target position is the same as the current position
                // So prevent error on origin cell
                return;
            }

            CarveWall(wallStates);
        }

        public void CarveWall(Vector4 wallStates)
        {
            ensureNodeChunkInstance();
            ChunkCell cell = _nodeChunkInstance.GetComponent<ChunkCell>() ?? throw new System.NullReferenceException("Node chunk instance does not have a ChunkCell component.");
            cell.Carve(wallStates);
        }

        public void ResetWalls()
        {
            ensureNodeChunkInstance();
            ChunkCell cell = _nodeChunkInstance.GetComponent<ChunkCell>() ?? throw new System.NullReferenceException("Node chunk instance does not have a ChunkCell component.");
            cell.activeWalls = new Vector4(1, 1, 1, 1); // Reset all walls to active
            cell.UpdateWalls();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (GetNodeDirection() < 0)
                Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetPosition(), 0.2f);

            if (GetNodeDirection() >= 0)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(GetFacingPosition(), 0.2f);
            }
            Gizmos.color = Color.white;
            Handles.Label(transform.position + new Vector3(0.5f, 0, 0.5f), _id);
        }
    }
}