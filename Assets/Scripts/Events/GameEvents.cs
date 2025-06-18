using System;

namespace Events
{
    public static class GameEvents
    {
        public static event Action PlayerDied;
        public static event Action PlayerRespawned;

        public static void OnPlayerDied()
        {
            PlayerDied?.Invoke();
        }

        public static void OnPlayerRespawned()
        {
            PlayerRespawned?.Invoke();
        }
    }
}
