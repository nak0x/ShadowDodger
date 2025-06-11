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
            currentCheckPointPosition = checkPoints[0].transform.position;
        }

        public void SetCurrentCheckPoint(GameObject checkPoint)
        {
            currentCheckPointPosition = checkPoint.transform.position;
            _lastCheckPointIndex = _currentCheckPointIndex;
            _currentCheckPointIndex = GetCheckPointIndex(checkPoint);
        }

        public GameObject GetLastCheckPoint()
        {
            return checkPoints[_lastCheckPointIndex];
        }

        private int GetCheckPointIndex(GameObject checkPoint)
        {
            for (int i = 0; i < checkPoints.Length; i++)
            {
                if (checkPoints[i] == checkPoint)
                    return i;
            }
            return -1;
        }
    }
}
