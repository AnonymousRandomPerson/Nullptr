using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary> Parent class for all GameObjects managed by EntityManager. </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary> Bool to keep track of whether or not this entity is dead. </summary>
        internal bool isDead;

        /// <summary> Initializes an entity to a starting transform. </summary>
        /// <param name="loc"> The transform to initialize to. </param>
        internal void Init(Transform loc)
        {
            transform.position = loc.position;
            transform.localRotation = loc.localRotation;
            transform.localScale = loc.localScale;
            isDead = false;
        }

        /// <summary> Moves the entity one time step. </summary>
        /// <param name="lifeTime"> How long this entity has been active. </param>
        internal abstract void Update(float lifeTime);

        /// <summary> Handles the death of an entity. </summary>
        internal virtual void Die()
        {
            transform.position = EntityManager.INIT_OBJECT_SPAWN;
            isDead = true;
        }
    }
}
