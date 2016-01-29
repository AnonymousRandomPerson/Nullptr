using UnityEngine;

namespace Assets.Scripts.Enemy.Boss
{
    class GarbageCollector : Enemy
    {
        [SerializeField]
        private float movementSpeed;
        [SerializeField]
        private float rotationSpeed;

        private float currentRotation;
        private bool rotatingLeft;

        public override void InitData()
        {
            base.InitData();
            currentRotation = 0f;
        }

        public override void RunEntity()
        {
            base.RunEntity();
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
        }


        void OnCollisionEnter2D(Collision2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "Enemy")
                    Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), coll.gameObject.GetComponent<Collider2D>());
                //else if (coll.gameObject.tag == "DestroyablePlatform")
                //{
                //    Platforms.DestroyablePlatform plat = coll.gameObject.GetComponent<Platforms.DestroyablePlatform>();
                //    if (plat != null)
                //        plat.DestroyThis();
                //}
            }
        }
    }
}
