using System.Net;
using UnityEngine;

namespace Level
{
    public class LevelCheckPointsManager : MonoBehaviour
    {
        [Header("LevelCheckPointsManager")]
        [SerializeField] private GameObject[] checkPoints;

        [System.NonSerialized]
        public Vector3 currentCheckPointPosition;
    
        private int _lastCheckPointIndex = 0;
        private int _currentCheckPointIndex = 0;

        void Start()
        {
            if (checkPoints.Length == 0)
                Debug.LogError("No checkpoints assigned into LevelCheckPointsManager");
            if (checkPoints.Length >= 1)
                currentCheckPointPosition = checkPoints[0].transform.position;
        }

        public void SetCurrentCheckPoint(GameObject checkPoint)
        {
            currentCheckPointPosition = checkPoint.transform.position;
            _lastCheckPointIndex = _currentCheckPointIndex;
            _currentCheckPointIndex = GetCheckPointIndex(checkPoint);
        }

        public void SetCurrentToLastCheckPoint()
        {
            currentCheckPointPosition = checkPoints[_lastCheckPointIndex].transform.position;
            _currentCheckPointIndex = _lastCheckPointIndex;
        }

        public GameObject GetLastCheckPoint()
        {
            return checkPoints[_lastCheckPointIndex];
        }

        private int GetCheckPointIndex(GameObject checkPoint)
        {
            return System.Array.IndexOf(checkPoints, checkPoint);
        }
    }
}
