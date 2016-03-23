using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;

namespace Assets.Scripts.Enemy.Enemies
{
    /// <summary>
    /// Stands still and shoots at the player.
    /// </summary>
    class RemoteEmberFox : VersionEnemy {

        /// <summary> The player that the enemy is aiming at. </summary>
        private GameObject player;
        /// <summary> The bullet manager used to acquire bullets from. </summary>
        private BulletManager bulletManager;

        /// <summary> The location of the enemy's gun. </summary>
        [SerializeField]
        [Tooltip("The location of the enemy's gun.")]
        private Transform gun;

        /// <summary> How close the player has to be before the enemy starts shooting it. </summary>
        [SerializeField]
        [Tooltip("How close the player has to be before the enemy starts shooting it.")]
        private float sightRange;
        /// <summary> The time taken for the enemy to reload after shooting. </summary>
        [SerializeField]
        [Tooltip("The time taken for the enemy to reload after shooting.")]
        private float reloadTime;
        /// <summary> The number of bullets the enemy fires per round. </summary>
        [SerializeField]
        [Tooltip("The number of bullets the enemy fires per round.")]
        private float bulletsPerRound;

        /// <summary> The number of bullets the enemy has fired in the current round. </summary>
        private float currentBullets;
        /// <summary> Timer for shooting bullets and reloading. </summary>
        private float shootTimer;

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
            if (bulletManager == null)
            {
                bulletManager = FindObjectOfType<Managers.BulletManager>();
            }

            currentBullets = 0;
            shootTimer = -reloadTime;
        }

        /// <summary>
        /// Entity Update Method. Replaces Update().
        /// </summary>
        public override void RunEntity()
        {
            base.RunEntity();

            float xOffset = player.transform.position.x - transform.position.x;
            float offsetMagnitude = Mathf.Abs(xOffset);
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * -Mathf.Sign(xOffset), transform.localScale.y);
            if (offsetMagnitude < sightRange && offsetMagnitude > Mathf.Abs(gun.transform.position.x - transform.position.x))
            {
                // Fire at the player if in range.
                if (shootTimer > currentBullets)
                {
                    Enums.Direction direction = xOffset > 0 ? Enums.Direction.Right : Enums.Direction.Left;
                    bulletManager.Shoot(Enums.BulletTypes.Enemy1, gun, direction);
                    if (++currentBullets >= bulletsPerRound)
                    {
                        // Reload after finishing a round.
                        currentBullets = 0;
                        shootTimer = -reloadTime;
                    }
                }
                shootTimer += Time.deltaTime;
            }
        }
    }
}