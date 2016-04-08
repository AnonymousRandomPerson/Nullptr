using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    /// <summary> Parent class for all GameObjects managed by EntityManager. </summary>
    public abstract class Entity : MonoBehaviour
    {
        /// <summary> Automaitcally triggers death for resetting from checkpoints. </summary>
        public static bool Reset = false;

        [SerializeField]
        protected string entityName;

        public string Name { get { return entityName; } }

        /// <summary> The total health of the Entity. </summary>
        [SerializeField]
        protected int totalHealth;
        public int TotalHealth { get { return totalHealth; } }
        /// <summary> The current health of the Entity. </summary>
        protected int currentHealth;
        public int CurrentHealth { get { return currentHealth; } }

        /// <summary> Reference to this entites manager for callbacks on death. </summary>
        private EntityManager manager;
        /// <summary> The type of this entity in the manager. </summary>
        private int type;
        /// <summary> The specific instance of this entity in the manager. </summary>
        private int instance;
        /// <summary> Allows this entity to run during a cutscene. </summary>
        private bool isCutScene;

        private bool paused;
        private float animSpeed;
        private float g;
        private Vector2 vel;

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
        internal void Init(Transform loc, EntityManager manager, int type, int instance, Enums.Direction direction, bool isCutScene)
        {
            transform.position = loc.position;
            transform.rotation = loc.rotation;
            if (isCutScene)
                transform.localScale = loc.localScale;
            this.manager = manager;
            this.type = type;
            this.instance = instance;
            this.direction = direction;
            this.isCutScene = isCutScene;
            paused = false;
            InitData();
        }

        void Update()
        {
            if (GameManager.IsRunning || (isCutScene && GameManager.IsCutScene))
            {
                if (paused)
                {
                    paused = false;
                    Animator anim = GetComponent<Animator>();
                    if (anim != null)
                        anim.speed = animSpeed;
                    Rigidbody2D rgby2D = GetComponent<Rigidbody2D>();
                    if (rgby2D != null)
                    {
                        rgby2D.gravityScale = g;
                        rgby2D.velocity = vel;
                    }
                }
                if (Reset)
                    Die();
                else
                    RunEntity();
            }
            else
            {
                if (!paused)
                {
                    Animator anim = GetComponent<Animator>();
                    if (anim != null)
                    {
                        animSpeed = anim.speed;
                        anim.speed = 0.0000001f;
                    }
                    Rigidbody2D rgby2D = GetComponent<Rigidbody2D>();
                    if (rgby2D != null)
                    {
                        g = rgby2D.gravityScale;
                        rgby2D.gravityScale = 0;
                        vel = rgby2D.velocity;
                        rgby2D.velocity = Vector2.zero;
                    }
                    paused = true;
                }
            }
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
            if (!manager.ReleaseEntity(type, instance))
                Debug.Log("Error Double free for " + gameObject.name + ": " + type + " " + instance);
        }
    }
}
