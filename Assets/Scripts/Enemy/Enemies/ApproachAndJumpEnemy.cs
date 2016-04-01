using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;

namespace Assets.Scripts.Enemy.Enemies
{
    class ApproachAndJumpEnemy : Enemy
    {
        [SerializeField]
        private float speed;

        // This much distance, or below, then jump. Check only x-axis distance.
        private float spaceToJump = 7;

        //location of player, to know when to jump
        private Transform playerLocTrans;

        //How many seconds from jump until explode
        private float timeToExplode = 1.5f;

        //Counts down to explode
        private float explodeTimer = 1000;

        //In air or not
		private bool inAir = false;

        //RigidBody, to be used for applying jump vector
        Rigidbody2D rBody;

		//When to start counting down for explosion
		bool startCounting = false;

        public override void InitData()
        {
            base.InitData();
            playerLocTrans = FindObjectOfType<Managers.PlayerManager>().GetPlayer().transform;
            rBody = GetComponent<Rigidbody2D>();
            explodeTimer = timeToExplode;
            startCounting = false;
            inAir = false;
        }

        public override void RunEntity()
        {
            base.RunEntity();
            if (!inAir)
            {
                transform.Translate(GetForward() * speed * Time.deltaTime);
            }

			if (startCounting && (explodeTimer -= Time.deltaTime) <= 0)
            {
                Explode();
            }
        }

        /// <summary>
        /// Causes the enemy to explode.
        /// </summary>
        private void Explode()
        {
            ExplosionManager.instance.SpawnExplosion(1, transform, Enums.Direction.None);
            Die ();
        }

        void FixedUpdate()
        {
            if (Mathf.Abs(playerLocTrans.position.x - ((Vector2)transform.position).x) <= spaceToJump
                && !inAir)
            {
                inAir = true;
                rBody.AddForce(new Vector2(GetForward().x * spaceToJump, spaceToJump), ForceMode2D.Impulse);
				startCounting = true;
            }
        }

        /// <summary>
        /// Explodes when the enemy hits the player.
        /// </summary>
        /// <param name="collision">The collision that the enemy was involved in.</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Player")
            {
                Explode();
            }
        }
    }
}