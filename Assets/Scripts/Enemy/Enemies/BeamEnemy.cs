using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;

namespace Assets.Scripts.Enemy.Enemies
{
    /// <summary>
    /// Waddle Doo clone, shoots a close-range beam periodically.
    /// </summary>
    class BeamEnemy : Enemy
    {
        /// <summary> The movement speed of the enemy. </summary>
        [SerializeField]
        private float speed;
        /// <summary> The delay between enemy attacks. </summary>
        [SerializeField]
        private float attackDelay;
        /// <summary> Timer until the enemy attacks. </summary>
        private float attackTimer;
        /// <summary> The time when the enemy started attacking. </summary>
        private float attackStartTime;
        /// <summary> The beam currently being shot by the enemy. </summary>
        private Entity beam;
        /// <summary> The bullet manager in the scene. </summary>
        private Managers.BulletManager bulletManager;
        /// <summary> The position where the beam will originate from. </summary>
        [SerializeField]
        private Transform wand;

        /// <summary>
        /// Method to allow custom data initialization.
        /// </summary>
        public override void InitData()
        {
            base.InitData();
            bulletManager = FindObjectOfType<Managers.BulletManager>();
            SetAttackTimer();
        }

        /// <summary>
        /// Entity Update Method. Replaces Update().
        /// </summary>
        public override void RunEntity()
        {
            base.RunEntity();
            if ((attackTimer -= Time.deltaTime) < 0)
            {
                if (attackTimer > -1)
                {
                    // Charge the attack.
                }
                else if (attackStartTime == 0)
                {
                    // Create a beam.
                    Enums.Direction direction = transform.localScale.x > 0 ? Enums.Direction.Left : Enums.Direction.Right;
                    bulletManager.Shoot(Enums.BulletTypes.Beam, wand, direction, ref beam);
                    attackStartTime = attackTimer;
                }
                else if (attackTimer < attackStartTime - 1)
                {
                    // Resume walking.
                    SetAttackTimer();
                    beam = null;
                }
            }
            else
            {
                bool inAir = false, blocked = false;
                TouchingSomething(ref inAir, ref blocked);
                if (blocked) {
                    Turn();
                }
                transform.Translate(GetForward() * speed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Randomly sets the timer for the enemy attack.
        /// </summary>
        private void SetAttackTimer()
        {
            attackStartTime = 0;
            attackTimer = attackDelay + Random.Range(-1f, 1f);
        }

        /// <summary>
        /// Handles the death of an entity.
        /// </summary>
        internal override void Die()
        {
            if (beam != null && beam.gameObject.activeSelf)
            {
                beam.Die();
            }
            base.Die();
        }
    }
}