using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class MallocAndFree : Enemy
    {
        /// <summary> How Fast this boss moves. </summary>
        [SerializeField]
        private float movementSpeed;
        /// <summary> Barrel for the gun. </summary>
        [SerializeField]
        private Transform barrel;
        [SerializeField]
        private Util.Enums.BulletTypes bullet;
        [SerializeField]
        private SpriteRenderer face;
        [SerializeField]
        private SpriteRenderer body;

        /// <summary> Signals the state machine the current animation is done. </summary>
        private bool animDone;
        private bool goFirst;
        private bool waitOnPartner;
        private bool sigWaitForAttack;
        private bool sigDone;
        private bool sigYourTurn;
        private bool sigDoSuper;
        /// <summary> Reference to the BulletManager for shooting. </summary>
        private Managers.BulletManager bulletManager;
        private float wait;
        private MallocAndFreeStateMachine machine;
        private MallocAndFreeStateMachine.State state;

        public bool GoFirst
        {
            set { goFirst = value; }
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
            machine = new MallocAndFreeStateMachine();
            wait = 0;
            FindObjectOfType<Platforms.GarbageFloor>().target = this.gameObject.transform;

        }

        public override void RunEntity()
        {
            MallocAndFreeStateMachine.State temp = state;
            // Get state
            state = machine.update(currentHealth, 5, animDone, goFirst, hit, waitOnPartner, sigWaitForAttack, sigDone, sigYourTurn, sigDoSuper);
            if (temp != state)
            {
                render = true;
                Render(true);
            }

            // Set up state vars
            if (animDone)
                animDone = false;

            // Run state
            switch (state)
            {
                case MallocAndFreeStateMachine.State.Intro: Intro(); break;
            }
        }

        protected override void Render(bool render)
        {
            face.enabled = render;
            body.enabled = render;
        }

        void Intro()
        {
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                
            }
        }
    }
}
