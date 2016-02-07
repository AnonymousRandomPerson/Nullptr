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

        /// <summary> How much the boss has rotated so far. </summary>
        private float currentRotation;
        /// <summary> Saves the current direction of rotation. </summary>
        private bool rotatingLeft;
        /// <summary> Reference to the BulletManager for shooting. </summary>
        private Managers.BulletManager bulletManager;

        public override void InitData()
        {
            base.InitData();
            currentRotation = 0f;
            bulletManager = FindObjectOfType<Managers.BulletManager>();
        }

        public override void RunEntity()
        {
            base.RunEntity();
            if(!Invincible)
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


        void OnCollisionEnter2D(Collision2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "Enemy")
                    Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), coll.gameObject.GetComponent<Collider2D>());
            }
        }
    }
}
