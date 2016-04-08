using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class KernelHand : Enemy
    {
        internal bool knockedOut;

        /// <summary> How Fast this boss moves. </summary>
        [SerializeField]
        private float movementSpeed;
        [SerializeField]
        private float hoverHeight;
        [SerializeField]
        private float introWaitTime;
        [SerializeField]
        private float waitTime;
        [SerializeField]
        private Vector3 pointA;
        [SerializeField]
        private Vector3 pointB;
        /// <summary> Barrel for the gun. </summary>
        [SerializeField]
        private Transform barrel;
        [SerializeField]
        private Util.Enums.BulletTypes bullet;
        [SerializeField]
        private SpriteRenderer body;
        [SerializeField]
        private Rigidbody2D rgby2d;
        [SerializeField]
        private Animator anim;

        private bool animDone;
        private bool done;
        private bool doOnce;
        private bool revive;
        private float wait;
        /// <summary> Reference to the BulletManager for shooting. </summary>
        private Managers.BulletManager bulletManager;
        private KernelHandStateMachine machine;
        private KernelHandStateMachine.State state;
        private GameObject player;
        /// <summary> The direction to travel in.</summary>
        private bool moveDirection;

        public override void InitData()
        {
            base.InitData();
            bulletManager = FindObjectOfType<Managers.BulletManager>();
            animDone = true;
            done = false;
            doOnce = false;
            revive = false;
            wait = 0;
            render = true;
            machine = new KernelHandStateMachine();
            player = FindObjectOfType<Managers.PlayerManager>().GetPlayer().gameObject;
        }

        public override void RunEntity()
        {
            KernelHandStateMachine.State temp = state;
            bool inAir = true, b = false;
            TouchingSomething(ref inAir, ref b);
            // Get state
            state = machine.update(currentHealth, done, hit && invulerability <= 0, revive);
            if (temp != state)
            {
                render = true;
                Render(true);
                done = false;
                doOnce = false;
                revive = false;
                wait = 0;
                //anim.SetBool("hover", false);
                //anim.SetBool("hurt", false);
                //anim.SetBool("attack", false);
                //anim.SetBool("slam", false);
                //anim.SetBool("idle", false);
                animDone = true;
                hit = false;
            }
            if (invulerability > 0)
            {
                render = !render;
                Render(render);
                invulerability -= Time.deltaTime;
            }
            else if (!render)
            {
                render = true;
                Render(true);
            }
            // Run state
            switch (state)
            {
                case KernelHandStateMachine.State.Intro: Intro(); break;
                case KernelHandStateMachine.State.Wait: Wait(); break;
                case KernelHandStateMachine.State.Hover: Hover(); break;
                case KernelHandStateMachine.State.Slam: Slam(); break;
                case KernelHandStateMachine.State.Drop: Drop(); break;
                case KernelHandStateMachine.State.Hit: Hit(); break;
                case KernelHandStateMachine.State.Dead: Dead(); break;
            }
        }

        public void AnimStep()
        {
            transform.Translate(-Vector2.right * .2f);
        }

        public void AnimDone()
        {
            animDone = true;
        }

        protected override void Render(bool render)
        {
            body.enabled = render;
            barrel.parent.GetComponent<SpriteRenderer>().enabled = render;
        }

        void Intro()
        {
            if ((wait += Time.deltaTime) > introWaitTime)
                done = true;
        }

        void Wait()
        {
            if (!doOnce)
            {
                //anim.SetBool("idle", true);
                doOnce = true;
            }
            if (transform.position.y < hoverHeight)
                transform.Translate(Vector2.up * 5 * Time.deltaTime);
            if (transform.position.x < pointA.x || transform.position.x > pointB.x)
                moveDirection = !moveDirection;
            if (moveDirection)
                transform.Translate(Vector2.right * Time.deltaTime * 2.5f);
            else
            {
                Vector3 directionLeft = transform.right;
                directionLeft.x = -directionLeft.x;
                transform.Translate(directionLeft * Time.deltaTime * 2.5f);
            }
            if ((wait += Time.deltaTime) > waitTime)
                done = true;
        }

        void Hover()
        {
            if (!doOnce)
            {
                //anim.SetBool("hover", true);
                doOnce = true;
            }
            float speedx, speedy;
            if (player.transform.position.x < transform.position.x)
                speedx = -movementSpeed * Time.deltaTime;
            else
                speedx = movementSpeed * Time.deltaTime;
            if (player.transform.position.y + 2 < transform.position.y)
                speedy = -movementSpeed * Time.deltaTime;
            else
                speedy = movementSpeed * Time.deltaTime;
            transform.Translate(new Vector3(speedx, speedy, 0));
            if ((wait += Time.deltaTime) > waitTime)
                done = true;
        }

        void Slam()
        {
            if (!doOnce)
            {
                rgby2d.gravityScale = 1;
                //anim.SetBool("slam", true);
                doOnce = true;
            }
            bool inAir = true, b = false;
            TouchingSomething(ref inAir, ref b);
            if (!inAir && animDone && rgby2d.velocity.y == 0)
            {
                done = true;
                rgby2d.gravityScale = 0;
            }
        }

        void Drop()
        {
            if (!doOnce)
            {
                //anim.SetBool("attack", true);
                bulletManager.Shoot(bullet, barrel, Util.Enums.Direction.Down);
                doOnce = true;
            }
            if (animDone)
                done = true;
        }

        void Hit()
        {
            if (!doOnce)
            {
                //anim.SetBool("hurt", true);
                doOnce = true;
            }
            if (invulerability <= 0)
            {
                currentHealth -= damage;
                invulerability = invulerabilityTime;
            }
            hit = false;
            if (animDone)
                done = true;
        }

        void Dead()
        {
            if (!doOnce)
            {
                rgby2d.gravityScale = 1;
                knockedOut = true;
                //anim.SetBool("hurt", true);
                doOnce = true;
            }
        }

        internal void Revive()
        {
            rgby2d.gravityScale = 0;
            revive = true;
            knockedOut = false;
            currentHealth = totalHealth;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "EnemyMalloc")
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), coll.collider);
            }
        }
    }
}
