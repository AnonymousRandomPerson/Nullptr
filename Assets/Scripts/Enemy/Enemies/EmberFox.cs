using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Enemy.Enemies
{
    /// <summary>
    /// An enemy that wanders randomly until the player gets close, then chases the player.
    /// Runs away when low on health.
    /// </summary>
    class EmberFox : VersionEnemy {

        /// <summary> The maximum movement speed of the enemy. </summary>
        [SerializeField]
        [Tooltip("The maximum movement speed of the enemy.")]
        private float maxSpeed;
        /// <summary> The turning speed of the enemy. </summary>
        [SerializeField]
        [Tooltip("The turning speed of the enemy.")]
        private float turnSpeed;
        /// <summary> The current movement speed of the enemy. </summary>
        private float currentSpeed;

        /// <summary> Whether the enemy is chasing the player. </summary>
        private bool chasing;
        /// <summary> How close the player has to be before the enemy starts chasing it. </summary>
        [SerializeField]
        [Tooltip("How close the player has to be before the enemy starts chasing it.")]
        private float sightRange;
        /// <summary> If the enemy has a lower fraction of health than this threshold, it will run away. </summary>
        [SerializeField]
        [Tooltip("If the enemy has a lower fraction of health than this threshold, it will run away.")]
        private float lowHealthThreshold;

        /// <summary> Time before the enemy changes its wander direction. </summary>
        private const float WANDERTIME = 2;
        /// <summary> Timer to control enemy wandering. </summary>
        private float wanderTimer;

        /// <summary> The player in the scene. </summary>
        private GameObject player;

        /// <summary>
        /// Method to allow custom data initialization.
        /// </summary>
        public override void InitData()
        {
            base.InitData();
            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>().GetPlayer().gameObject;
            }

            currentSpeed = 0;
            chasing = false;
            wanderTimer = 0;
        }

        /// <summary>
        /// Entity Update Method. Replaces Update().
        /// </summary>
        public override void RunEntity()
        {
            base.RunEntity();
            float xOffset = player.transform.position.x - transform.position.x;
            if (currentHealth < totalHealth || Mathf.Abs(xOffset) < sightRange)
            {
                chasing = true;
            }
            if (chasing)
            {
                // Chase the player.
                float direction = Mathf.Sign(xOffset) * maxSpeed;
                if (currentHealth < totalHealth * lowHealthThreshold)
                {
                    // Run away if low on health.
                    direction = -direction;
                }
                currentSpeed = Mathf.MoveTowards(currentSpeed, direction, turnSpeed);
            }
            else
            {
                // Wander.
                wanderTimer -= Time.deltaTime;
                if (wanderTimer < 0)
                {
                    currentSpeed = Random.Range(-1, 2) * maxSpeed / 4;
                    wanderTimer = WANDERTIME * Random.Range(0.5f, 1.5f);
                }
            }

            transform.Translate(new Vector2(currentSpeed * Time.deltaTime, 0));
        }
    }
}