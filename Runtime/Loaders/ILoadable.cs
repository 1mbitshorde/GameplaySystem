using UnityEngine;

namespace OneM.GameplaySystem
{
    /// <summary>
    /// Interface used on objects able to be loaded asynchronously.
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        /// Loads the object asynchronously.
        /// </summary>
        /// <returns>An asynchronous operation.</returns>
        Awaitable LoadAsync();
    }
}