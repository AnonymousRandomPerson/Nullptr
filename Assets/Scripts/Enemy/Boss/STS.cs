using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class STS : Enemy
    {
        /// <summary> How Fast this boss moves. </summary>
        [SerializeField]
        private float movementSpeed;
        [SerializeField]
        private float introWaitTime;
        [SerializeField]
        private float waitTime;
        [SerializeField]
        private float jumpForce;
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
        private bool jump;
        private float wait;
        /// <summary> Reference to the BulletManager for shooting. </summary>
        private Managers.BulletManager bulletManager;
        private STSStateMachine machine;
        private STSStateMachine.State state;
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
            jump = false;
            wait = 0;
            render = true;
            machine = new STSStateMachine();
            player = FindObjectOfType<Managers.PlayerManager>().GetPlayer().gameObject;
        }

        public override void RunEntity()
        {
            STSStateMachine.State temp = state;
            // Get state
            state = machine.update(currentHealth, done, hit && invulerability <= 0);
            if (temp != state)
            {
                if (player.transform.position.x > transform.position.x)
                    FaceLeft();
                else
                    FaceRight();
                render = true;
                Render(true);
                done = false;
                doOnce = false;
                jump = false;
                wait = 0;
                //anim.SetBool("walk", false);
                //anim.SetBool("hurt", false);
                //anim.SetBool("attack", false);
                //anim.SetBool("jump", false);
                //anim.SetBool("idle", false);
                animDone = true;
                hit = false;
                //if (state == MallocAndFreeStateMachine.State.AirAttack)
                //    partner.SigWaitForAttack = true;
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
                case STSStateMachine.State.Intro: Intro(); break;
                case STSStateMachine.State.Wait: Wait(); break;
                case STSStateMachine.State.Step: Step(); break;
                case STSStateMachine.State.Jump: Jump(); break;
                case STSStateMachine.State.GroundAttack: GroundAttack(); break;
                case STSStateMachine.State.Hit: Hit(); break;
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
            if ((wait += Time.deltaTime) > waitTime)
                done = true;
        }

        void Step()
        {
            if (!doOnce)
            {
                //anim.SetBool("walk", true);
                doOnce = true;
            }
            if (animDone)
                done = true;
        }

        void Jump()
        {
            if (!doOnce)
            {
                jump = true;
                //anim.SetBool("jump", true);
                doOnce = true;
            }
            bool inAir = true, b = false;
            TouchingSomething(ref inAir, ref b);
            if (!inAir && animDone && rgby2d.velocity.y == 0)
                done = true;
        }

        void GroundAttack()
        {
            if (!doOnce)
            {
                //anim.SetBool("attack", true);
                bulletManager.Shoot(bullet, barrel,
                    transform.localScale.x < 0 ? Util.Enums.Direction.Left : Util.Enums.Direction.Right);
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
            if (currentHealth <= 0)
            {
                Render(true);
                Die();
            }
            if (animDone)
                done = true;
        }

        void FixedUpdate()
        {
            if (jump)
            {
                rgby2d.AddForce(new Vector2(transform.localScale.x * .70f, 1) * jumpForce, ForceMode2D.Impulse);
                jump = false;
            }
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
            }
        }
    }
}
