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

        //Jump vector
        private Vector2 jumpVec = new Vector2(-7, 7);

        //location of player, to know when to jump
        private Transform playerLocTrans;

        //How many seconds from jump until explode
        private float timeToExplode = 1f;

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
        }

        public override void RunEntity()
        {
            base.RunEntity();
            if (!inAir)
            {
                transform.Translate(-1 * GetForward() * speed * Time.deltaTime);
            }

			if (startCounting && (explodeTimer -= Time.deltaTime) <= 0)
            {
				ExplosionManager.instance.SpawnExplosion(1, transform, Enums.Direction.None);
				Die ();
            }
        }

        void FixedUpdate()
        {
            if (Mathf.Abs(playerLocTrans.position.x - ((Vector2)transform.position).x) <= spaceToJump
                && !inAir)
            {
                inAir = true;
                rBody.AddForce(jumpVec, ForceMode2D.Impulse);
                explodeTimer = timeToExplode;
				startCounting = true;
            }
        }
    }
}