using System;
using UnityEngine;

namespace OneM.GameplaySystem
{
    /// <summary>
    /// Respawner component.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Respawner : MonoBehaviour, IRespawnable
    {
        /// <summary>
        /// The last saved place.
        /// </summary>
        /// <remarks>
        /// You can set it using a transform: <c>respawner.LastPlace = transform;</c>
        /// </remarks>
        public Place LastSavedPlace;

        public event Action OnRespawned;

        private void Awake() => LastSavedPlace = transform;

        public void Respawn()
        {
            LastSavedPlace.Set(transform);
            OnRespawned?.Invoke();
        }
    }
}