using System;
using System.Linq;
using UnityEngine;
using ActionCode.PauseSystem;
using ActionCode.ScreenFadeSystem;

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
        [SerializeField, Tooltip("The Prefab containing an instance of AbstractScreenFader. It'll be used when respawning.")]
        private AbstractScreenFader screenFaderPrefab;

        /// <summary>
        /// The current Screen Fader.
        /// </summary>
        public static AbstractScreenFader Fader { get; private set; }

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

        private void Awake() => Fader = ScreenFadeFactory.Create(screenFaderPrefab);
        private void OnDestroy() => Dispose();

        /// <summary>
        /// Whether there is a Player instance set.
        /// </summary>
        /// <returns></returns>
        public static bool HasPlayer() => Player != null;

        /// <summary>
        /// Respawns the game at the given time.
        /// </summary>
        /// <remarks>
        /// Respawns all instances implementing <see cref="IRespawnable"/>.
        /// </remarks>
        /// <param name="time">Time to respawn.</param>
        public static async Awaitable RespawnAsync(float time)
        {
            CurrentState = State.Respawn;

            await Awaitable.WaitForSecondsAsync(time);
            if (Fader) await Fader.FadeOutAsync();

            await Awaitable.WaitForSecondsAsync(1f);

            var respawns = FindObjectsByInterface<IRespawnable>();
            foreach (var respawnable in respawns)
            {
                respawnable.Respawn();
            }

            if (Fader) await Fader.FadeInAsync();

            CurrentState = State.Gameplay;
        }

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

        private static T[] FindObjectsByInterface<T>() where T : class
        {
            var monos = FindObjectsByType<MonoBehaviour>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );
            return monos.OfType<T>().ToArray();
        }
    }
}