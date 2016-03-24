using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Enemy.Enemies
{
    /// <summary>
    /// Enemy that flies towards the player.
    /// </summary>
    class FlyingEnemy : Enemy
    {

        /// <summary> The movement speed of the enemy. </summary>
        [SerializeField]
        [Tooltip("The movement speed of the enemy.")]
        private float moveSpeed;
        /// <summary> The turning speed of the enemy. </summary>
        [SerializeField]
        [Tooltip("The turning speed of the enemy.")]
        private float turnSpeed;
        /// <summary> The direction the enemy is traveling in. </summary>
        private Vector3 moveDirection;

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
                player = FindObjectOfType<Managers.PlayerManager>().GetPlayer().gameObject;
            }
        }

        /// <summary>
        /// Entity Update Method. Replaces Update().
        /// </summary>
        public override void RunEntity()
        {
            base.RunEntity();
            Vector3 targetDirection = Vector3.Normalize(player.transform.position - transform.position);
            moveDirection = Vector3.MoveTowards(moveDirection, targetDirection, turnSpeed);
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}
