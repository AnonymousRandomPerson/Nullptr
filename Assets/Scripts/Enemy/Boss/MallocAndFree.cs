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
        private Transform barrel;
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

        private bool animDone;
        private bool done;
        private bool doOnce;
        private bool jump;
        private bool goFirst;
        private bool waitOnPartner;
        private bool sigWaitForAttack;
        private bool sigDone;
        private bool sigYourTurn;
        private bool sigDoSuper;
        private float wait;
        /// <summary> Reference to the BulletManager for shooting. </summary>
        private Managers.BulletManager bulletManager;
        private MallocAndFreeStateMachine machine;
        private MallocAndFreeStateMachine.State state;
        private GameObject player;
        private MallocAndFree partner;
        /// <summary> The direction to travel in.</summary>
        private bool moveDirection;

        public bool GoFirst
        {
            set { goFirst = value; }
        }

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
            set { SigYourTurn = value; }
        }
        private bool SigDoSuper
        {
            set { sigDoSuper = value; }
        }

        public override void InitData()
        {
            base.InitData();
            bulletManager = FindObjectOfType<Managers.BulletManager>();
            animDone = false;
            done = false;
            doOnce = false;
            jump = false;
            goFirst = false;
            waitOnPartner = false;
            sigWaitForAttack = false;
            sigDone = false;
            sigYourTurn = false;
            sigDoSuper = false;
            wait = 0;
            machine = new MallocAndFreeStateMachine();
            FindObjectOfType<Platforms.GarbageFloor>().target = this.gameObject.transform;
            player = FindObjectOfType<Managers.PlayerManager>().GetPlayer().gameObject;
        }

        public override void RunEntity()
        {
            MallocAndFreeStateMachine.State temp = state;
            // Get state
            state = machine.update(currentHealth, 5, done, goFirst, hit, waitOnPartner, sigWaitForAttack, sigDone, sigYourTurn, sigDoSuper);
            if (temp != state)
            {
                if (player.transform.position.x < transform.position.x)
                    FaceLeft();
                else
                    FaceRight();
                render = true;
                Render(true);
                done = false;
                doOnce = false;
                jump = false;
                goFirst = false;
                waitOnPartner = false;
                sigWaitForAttack = false;
                sigDone = false;
                sigYourTurn = false;
                sigDoSuper = false;
                wait = 0;
                hit = false;
                if (state == MallocAndFreeStateMachine.State.AirAttack)
                    partner.SigWaitForAttack = true;
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
                case MallocAndFreeStateMachine.State.Super: Super(); break;
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
            bool inAir = true, b = false;
            TouchingSomething(ref inAir, ref b);
            if (!inAir)
                done = true;
        }

        void GoToAir()
        {
            rgby2d.gravityScale = 0;
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
                bulletManager.Shoot(Util.Enums.BulletTypes.Enemy1, barrel, Util.Enums.Direction.Right);
            else
                bulletManager.Shoot(Util.Enums.BulletTypes.Enemy2, barrel, Util.Enums.Direction.None);
            done = true;
        }

        void Move()
        {
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

        void AirAttack()
        {
            if (isMalloc)
                bulletManager.Shoot(Util.Enums.BulletTypes.Enemy1, barrel, Util.Enums.Direction.Right);
            else
                bulletManager.Shoot(Util.Enums.BulletTypes.Enemy2, barrel, Util.Enums.Direction.None);
            done = true;
        }

        void FixedUpdate()
        {
            if(jump)
            {
                rgby2d.AddForce(new Vector2(1, 1) * jumpForce, ForceMode2D.Impulse);
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
