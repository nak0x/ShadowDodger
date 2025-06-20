using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
    public abstract class ResetableMonoBehaviour : MonoBehaviour
    {
        public abstract void ResetProperty(Player.PlayerResetType resetType = Player.PlayerResetType.Medium);
    }
}