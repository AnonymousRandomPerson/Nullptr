using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class GarbageCollector : Enemy
    {
        /// <summary> How Fast this boss moves. </summary>
        [SerializeField]
        private float movementSpeed;
        /// <summary> Barrel for the gun. </summary>
        [SerializeField]
        private Transform barrel;
        /// <summary> Barrel for the gun. </summary>
        [SerializeField]
        private Transform skyBarrel;
        /// <summary> Barrel for the gun. </summary>
        [SerializeField]
        private Transform[] barrels;
        /// <summary> Place to move to during intro. </summary>
        [SerializeField]
        private int startLoc;
        [SerializeField]
        private SpriteRenderer sprite;

        /// <summary> Signals the state machine the current animation is done. </summary>
        private bool animDone;
        /// <summary> Signals the state machine to start the super. </summary>
        private bool superStart;
        /// <summary> Reference to the BulletManager for shooting. </summary>
        private Managers.BulletManager bulletManager;
        private int platformsAte;
        private int count;
        private float wait;
        private GarbageCollectorStateMachine machine;

        public override void InitData()
        {
            base.InitData();
            bulletManager = FindObjectOfType<Managers.BulletManager>();
            animDone = false;
            superStart = false;
            platformsAte = 0;
            count = 0;
            machine = new GarbageCollectorStateMachine();
            wait = 0;
            FindObjectOfType<Platforms.GarbageFloor>().target = this.gameObject.transform;
        }

        public override void RunEntity()
        {
            if (platformsAte > 9)
            {
                superStart = true;
                wait = 0f;
                platformsAte = 0;
            }
            // Get state
            GarbageCollectorStateMachine.State state = machine.update(animDone, superStart );

            // Set up state vars
            if(animDone)
                animDone = false;
            if (superStart)
                superStart = false;

            // Run state
            switch(state)
            {
                case GarbageCollectorStateMachine.State.Intro: Intro(); break;
                case GarbageCollectorStateMachine.State.General: General(); break;
                case GarbageCollectorStateMachine.State.SuperStart: SuperStart(); break;
                case GarbageCollectorStateMachine.State.SuperWait: SuperWait(); break;
            }
        }

        protected override void Render(bool render)
        {
            sprite.enabled = render;
        }

        void Intro()
        {
            if (transform.position.x > startLoc)
                animDone = true;
            transform.Translate(GetForward() * movementSpeed * 1.2f * Time.deltaTime);
        }

        void General()
        {
            if (hit)
            {
                if (invulerability <= 0)
                {
                    currentHealth--;
                    invulerability = invulerabilityTime;
                }
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
            if (currentHealth <= 0)
            {
                Render(true);
                Die();
            }
            transform.Translate(GetForward() * movementSpeed * Time.deltaTime);
            //if (Random.Range(0.0f, 1.0f) < .05f)
            //    bulletManager.Shoot(Util.Enums.BulletTypes.Enemy, barrel, Util.Enums.Direction.Right);
        }

        void SuperStart()
        {
            if (count != 8 && wait > .2f)
                wait = 0f;
            if (wait == 0f)
            {
                bulletManager.Shoot(Util.Enums.BulletTypes.Enemy, skyBarrel, Util.Enums.Direction.Right);
                count++;
            }
            wait += Time.deltaTime;
            if(count == 8 && wait > 1f)
            {
                wait = 0;
                count = 0;
                animDone = true;
            }
        }

        void SuperWait()
        {
            if (wait > 1.5f)
                wait = 0f;
            if (wait == 0f)
            {
                foreach (Transform b in barrels)
                    bulletManager.Shoot(Util.Enums.BulletTypes.Enemy, b, Util.Enums.Direction.Down);
                count++;
            }
            wait += Time.deltaTime;
            if (count == 2 && wait > 1f)
            {
                wait = 0;
                count = 0;
                animDone = true;
            }
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "DestroyablePlatform")
                {
                    platformsAte++;
                }
            }
        }
    }
}
