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
        [SerializeField]
        private float spaceToJump;

        //Jump vector
        private Vector2 jumpVec = new Vector2(-40, 60);

        //location of player, to know when to jump
        private Transform playerLocTrans;

        //How many seconds from jump until explode
        [SerializeField]
        private float timeToExplode = 2;

        //Counts down to explode
        private float explodeTimer;

        //In air or not
        private bool inAir;

        //RigidBody, to be used for applying jump vector
        Rigidbody2D rBody;

        public override void InitData()
        {
            base.InitData();
            playerLocTrans = FindObjectOfType<Managers.PlayerManager>().GetPlayer().transform;
			Debug.Log ("Got player transform as " + playerLocTrans);
            rBody = GetComponent<Rigidbody2D>();
        }

        public override void RunEntity()
        {
            base.RunEntity();
            if (!inAir)
            {
                transform.Translate(GetForward() * speed * Time.deltaTime);
            }

            if ((explodeTimer -= Time.deltaTime) <= 0)
            {
                Debug.Log("Destroyed");
                //Create explosion with value 2 to hurt player
				//for now use 0
				ExplosionManager.instance.SpawnExplosion(2, transform, Enums.Direction.None);
				Debug.Log("I HAVE EXPLODED");
                Destroy(this);
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
            }
        }
    }
}