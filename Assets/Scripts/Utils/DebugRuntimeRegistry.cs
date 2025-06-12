using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
    public class DebugRuntimeRegistry : MonoBehaviour
    {
        public List<MonoBehaviour> debugTargets;
    }

    public interface IDevSerializable
    {
        public string DevSerialize();
    }
}