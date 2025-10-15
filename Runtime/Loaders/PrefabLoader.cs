using UnityEngine;
using UnityEngine.AddressableAssets;

namespace OneM.GameplaySystem
{
    /// <summary>
    /// Loads and unloads a prefab using Addressables.
    /// </summary>
    public sealed class PrefabLoader : MonoBehaviour
    {
        [SerializeField] private AssetReferenceGameObject prefab;

        private GameObject instance;

        public bool IsLoaded() => instance != null;

        public async Awaitable LoadAsync() =>
            instance = await Addressables.InstantiateAsync(prefab, transform).Task;

        public void Unload()
        {
            Addressables.ReleaseInstance(instance);
            instance = null;
        }
    }
}