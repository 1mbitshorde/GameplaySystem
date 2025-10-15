using UnityEngine;
using ActionCode.SceneManagement;

namespace OneM.GameplaySystem
{
    /// <summary>
    /// Loads a series of <see cref="ILoadable"/> implementations as Game Objects children, 
    /// one at a time, in the order they are in the hierarchy as children.
    /// </summary>
    public class GameLoader : AbstractLoader
    {
        protected override async Awaitable LoadAsync()
        {
            GameplayManager.CurrentState = State.Loading;

            var loadables = GetComponentsInChildren<ILoadable>();
            foreach (var loadable in loadables)
            {
                await loadable.LoadAsync();
            }

            //TODO check if in Cutscene
            GameplayManager.CurrentState = State.Gameplay;
        }
    }
}