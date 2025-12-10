using UnityEngine;

namespace OneM.GameplaySystem
{
    /// <summary>
    /// Position and rotation representation.
    /// </summary>
    [System.Serializable]
    public struct Place
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public Place(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Place(Transform transform) :
            this(transform.position, transform.rotation)
        { }

        public readonly void Set(Transform transform) => transform.SetPositionAndRotation(Position, Rotation);

        public static implicit operator Place(Transform transform) => new(transform);
    }
}