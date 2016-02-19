using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class GarbageCollector : Enemy
    {
        /// <summary> How Fast this boss moves. </summary>
        [SerializeField]
        private float movementSpeed;
        /// <summary> How fast this boss rotates back and forth. </summary>
        [SerializeField]
        private float rotationSpeed;
        /// <summary> Barrel for the gun. </summary>
        [SerializeField]
        private Transform barrel;
        /// <summary> Place to move to during intro. </summary>
        [SerializeField]
        private int startLoc;

        /// <summary> How much the boss has rotated so far. </summary>
        private float currentRotation;
        /// <summary> Saves the current direction of rotation. </summary>
        private bool rotatingLeft;
        /// <summary> Signals the state machine the current animation is done. </summary>
        private bool animDone;
        /// <summary> Signals the state machine to start the super. </summary>
        private bool superStart;
        /// <summary> Reference to the BulletManager for shooting. </summary>
        private Managers.BulletManager bulletManager;

        private GarbageCollectorStateMachine machine;

        public override void InitData()
        {
            base.InitData();
            currentRotation = 0f;
            bulletManager = FindObjectOfType<Managers.BulletManager>();
            rotatingLeft = false;
            animDone = false;
            superStart = false;
            machine = new GarbageCollectorStateMachine();
        }

        public override void RunEntity()
        {
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
            if (!Invincible)
                transform.Translate(GetForward() * movementSpeed * Time.deltaTime);
            if (rotatingLeft)
            {
                transform.Rotate(new Vector3(0, 0, -rotationSpeed));
                currentRotation -= rotationSpeed;
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, rotationSpeed));
                currentRotation += rotationSpeed;
            }
            if (currentRotation < -30)
                rotatingLeft = false;
            if (currentRotation > 30)
                rotatingLeft = true;
            if (Random.Range(0.0f, 1.0f) < .05f)
                bulletManager.Shoot(Util.Enums.BulletTypes.Destroyer, barrel, Util.Enums.Direction.Right);
        }

        void SuperStart()
        {

        }

        void SuperWait()
        {

        }
    }
}
