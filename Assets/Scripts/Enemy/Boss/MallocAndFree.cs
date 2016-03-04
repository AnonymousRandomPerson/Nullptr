using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class MallocAndFree : Enemy
    {
        [SerializeField]
        private bool isMalloc;
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
        private float jumpForce;
        /// <summary> Barrel for the gun. </summary>
        [SerializeField]
        private Transform[] barrel;
        [SerializeField]
        private Vector3 pointA;
        [SerializeField]
        private Vector3 pointB;
        [SerializeField]
        private Util.Enums.BulletTypes bullet;
        [SerializeField]
        private SpriteRenderer face;
        [SerializeField]
        private SpriteRenderer body;
        [SerializeField]
        private Rigidbody2D rgby2d;
        [SerializeField]
        private bool goFirst;

        private bool animDone;
        private bool done;
        private bool doOnce;
        private bool jump;
        private bool waitOnPartner;
        private bool sigWaitForAttack;
        private bool sigDone;
        private bool sigYourTurn;
        private bool stage1;
        private float wait;
        /// <summary> Reference to the BulletManager for shooting. </summary>
        private Managers.BulletManager bulletManager;
        private MallocAndFreeStateMachine machine;
        private MallocAndFreeStateMachine.State state;
        private GameObject player;
        private MallocAndFree partner;
        /// <summary> The direction to travel in.</summary>
        private bool moveDirection;

        public MallocAndFree Partner
        {
            set { partner = value; }
        }

        private bool SigWaitForAttack
        {
            set { sigWaitForAttack = value; }
        }
        private bool SigDone
        {
            set { sigDone = value; }
        }
        private bool SigYourTurn
        {
            set { sigYourTurn = value; }
        }

        public override void InitData()
        {
            base.InitData();
            bulletManager = FindObjectOfType<Managers.BulletManager>();
            animDone = true;
            done = false;
            doOnce = false;
            jump = false;
            waitOnPartner = false;
            sigWaitForAttack = false;
            sigDone = false;
            sigYourTurn = false;
            stage1 = true;
            wait = 0;
            render = true;
            machine = new MallocAndFreeStateMachine();
            player = FindObjectOfType<Managers.PlayerManager>().GetPlayer().gameObject;
            face.enabled = false;
        }

        public override void RunEntity()
        {
            MallocAndFreeStateMachine.State temp = state;
            // Get state
            state = machine.update(currentHealth, 5, done, goFirst, hit && invulerability <= 0, waitOnPartner, sigWaitForAttack, sigDone, sigYourTurn);
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
                waitOnPartner = false;
                sigWaitForAttack = false;
                sigDone = false;
                sigYourTurn = false;
                wait = 0;
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
                case MallocAndFreeStateMachine.State.Intro: Intro(); break;
                case MallocAndFreeStateMachine.State.GoToGround: GoToGround(); break;
                case MallocAndFreeStateMachine.State.GoToAir: GoToAir(); break;
                case MallocAndFreeStateMachine.State.Wait: Wait(); break;
                case MallocAndFreeStateMachine.State.Step: Step(); break;
                case MallocAndFreeStateMachine.State.Jump: Jump(); break;
                case MallocAndFreeStateMachine.State.GroundAttack: GroundAttack(); break;
                case MallocAndFreeStateMachine.State.Move: Move(); break;
                case MallocAndFreeStateMachine.State.AirAttack: AirAttack(); break;
                case MallocAndFreeStateMachine.State.Stage2: Stage2(); break;
                case MallocAndFreeStateMachine.State.Hit: Hit(); break;
            }
        }

        public void AnimStep()
        {
            transform.Translate(Vector2.right * .2f);
        }

        public void AnimDone()
        {
            animDone = true;
        }

        protected override void Render(bool render)
        {
            face.enabled = render;
            body.enabled = render;
        }

        void Intro()
        {
            if ((wait += Time.deltaTime) > introWaitTime)
                done = true;
        }

        void GoToGround()
        {
            rgby2d.gravityScale = 1;
            face.enabled = true;
            bool inAir = true, b = false;
            TouchingSomething(ref inAir, ref b);
            if (!inAir)
                done = true;
        }

        void GoToAir()
        {
            rgby2d.gravityScale = 0;
            rgby2d.velocity = Vector2.zero;
            face.enabled = false;
            partner.SigYourTurn = true;
            if (transform.position.y < hoverHeight)
                transform.Translate(Vector2.up * 5 * Time.deltaTime);
            else
                done = true;
        }

        void Wait()
        {
            if ((wait += Time.deltaTime) > waitTime)
                done = true;
        }

        void Step()
        {
            if (animDone)
                done = true;
        }

        void Jump()
        {
            if(!doOnce)
            {
                jump = true;
                doOnce = true;
            }
            bool inAir = true, b = false;
            TouchingSomething(ref inAir, ref b);
            if (!inAir && animDone)
                done = true;
        }

        void GroundAttack()
        {
            if (isMalloc)
                bulletManager.Shoot(Util.Enums.BulletTypes.Enemy1, barrel[0], transform.localScale.x < 0 ? Util.Enums.Direction.Left : Util.Enums.Direction.Right);
            else
                bulletManager.Shoot(Util.Enums.BulletTypes.Enemy2, barrel[0], Util.Enums.Direction.None);
            done = true;
        }

        void Move()
        {
            face.enabled = false;
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
            if ((wait += Time.deltaTime) > waitTime * 3)
                done = true;
        }

        void AirAttack()
        {
            if (isMalloc)
            {
                for (int i = 0; i < barrel.Length / 2; i++)
                    bulletManager.Shoot(Util.Enums.BulletTypes.Enemy1, barrel[i], Util.Enums.Direction.Right);
                for (int i = barrel.Length / 2; i < barrel.Length; i++)
                    bulletManager.Shoot(Util.Enums.BulletTypes.Enemy1, barrel[i], Util.Enums.Direction.Left);
            }
            else
                for (int i = 0; i < barrel.Length; i++)
                    bulletManager.Shoot(Util.Enums.BulletTypes.Enemy2, barrel[i], Util.Enums.Direction.None);
            done = true;
        }
        
        void Stage2()
        {
            if (sigYourTurn)
                waitOnPartner = false;
            else
                waitOnPartner = true;
            partner.sigYourTurn = true;
            currentHealth = totalHealth/2;
        }

        void Hit()
        {
            if (invulerability <= 0)
            {
                currentHealth -= damage;
                invulerability = invulerabilityTime;
            }
            hit = false;
            if (currentHealth <= 0)
            {
                Render(true);
                if (stage1)
                    stage1 = false;
                else
                    Die();
            }
            done = true;
        }

        void FixedUpdate()
        {
            if(jump)
            {
                rgby2d.AddForce(new Vector2(transform.localScale.x * .75f, 1) * jumpForce, ForceMode2D.Impulse);
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
