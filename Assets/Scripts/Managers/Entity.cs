using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    /// <summary> Parent class for all GameObjects managed by EntityManager. </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary> Reference to this entites manager for callbacks on death. </summary>
        private EntityManager manager;
        /// <summary> The type of this entity in the manager. </summary>
        private int type;
        /// <summary> The specific instance of this entity in the manager. </summary>
        private int instance;

        /// <summary> Reference to this entites manager for callbacks on death. </summary>
        protected EntityManager Manager
        {
            get { return manager; }
        }
        /// <summary> The type of this entity in the manager. </summary>
        protected int Type
        {
            get { return type; }
        }
        /// <summary> The specific instance of this entity in the manager. </summary>
        protected int Instance
        {
            get { return instance; }
        }
        
        /// <summary> The direction this entity is facing at init. </summary>
        protected Enums.Direction direction;

        /// <summary> Initializes an entity to a starting transform. </summary>
        /// <param name="loc"> The transform to initialize to. </param>
        /// <param name="manager"> Reference to this entites manager for callbacks on death. </param>
        /// <param name="type"> The type of this entity in the manager. </param>
        /// <param name="instance"> The specific instance of this entity in the manager. </param>
        /// <param name="direction"> The direction this entity is facing at init. </param>
        internal void Init(Transform loc, EntityManager manager, int type, int instance, Enums.Direction direction)
        {
            transform.position = loc.position;
            transform.localRotation = loc.rotation;
            this.manager = manager;
            this.type = type;
            this.instance = instance;
            this.direction = direction;
            InitData();
        }

        void Update()
        {
            if(GameManager.IsRunning)
                RunEntity();
        }

        /// <summary> Method to allow custom data initialization. </summary>
        public abstract void InitData();

        /// <summary> Entity Update Method. Replaces Update(). </summary>
        public abstract void RunEntity();

        /// <summary> Called when a collision is detected by another entity. </summary>
        /// <param name="col"> The entity colliding with this one. </param>
        public abstract void HitByEntity(Entity col);

        /// <summary> Handles the death of an entity. </summary>
        internal virtual void Die()
        {
            manager.ReleaseEntity(type, instance);
        }
    }
}
