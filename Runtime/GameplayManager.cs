using System;
using UnityEngine;
using ActionCode.PauseSystem;

namespace OneM.GameplaySystem
{
    /// <summary>
    /// The global manager for Gameplay.
    /// </summary>
    /// <remarks>
    /// Put this component inside you Game Scene and set <see cref="CurrentState"/> 
    /// for all Gameplay states in your game.<br/>
    /// You can listen for those state changes by subscribing to the event <see cref="OnStateChanged"/>.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class GameplayManager : MonoBehaviour
    {
        /// <summary>
        /// The single Player instance.
        /// </summary>
        public static GameObject Player { get; set; }

        /// <summary>
        /// The last Game State.
        /// </summary>
        public static State LastState { get; private set; }

        /// <summary>
        /// The current Game State.
        /// </summary>
        public static State CurrentState
        {
            get => currentState;
            set
            {
                var hasSameValue = currentState == value;
                if (hasSameValue) return;

                LastState = currentState;
                currentState = value;

                UpdadeGameState();
                OnStateChanged?.Invoke(currentState);
            }
        }

        /// <summary>
        /// Event fired when the Gameplay state changes.
        /// </summary>
        public static event Action<State> OnStateChanged;

        private static State currentState;

        private void OnDestroy() => Dispose();

        private static void Dispose()
        {
            Player = null;
            LastState = default;
            currentState = default;
        }

        private static void UpdadeGameState()
        {
            switch (CurrentState)
            {
                case State.Gameplay:
                    PauseManager.Resume();
                    break;

                case State.Pause:
                case State.Debug:
                case State.Menu:
                    PauseManager.Pause();
                    break;

                case State.Loading:
                case State.Cutscene:
                case State.Dialogue:
                    break;
            }
        }
    }
}