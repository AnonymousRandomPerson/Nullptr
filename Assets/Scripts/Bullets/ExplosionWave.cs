using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Bullets
{
    class ExplosionWave : Entity, Bullet
    {
        [SerializeField]
        private int damage;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float raycastRadius;
        [SerializeField]
        private string targetTag;
        [SerializeField]
        private float lifeTime;
        [SerializeField]
        private LayerMask rayCastLayer;
        [SerializeField]
        private Transform rayCastPoint;

        private float currentLifeTime;
        private float currentRaycastRadius;
        private float currentSizeX;
        private float currentSizeY;
        private float finalSizeX;
        private float finalSizeY;


        public int getDamage()
        {
            return damage;
        }

        public override void InitData()
        {
            currentLifeTime = 0;
            currentRaycastRadius = 0;
            currentSizeX = 0;
            currentSizeY = 0;
            finalSizeX = transform.localScale.x;
            finalSizeY = transform.localScale.y;
            transform.localScale = Vector3.zero;
            transform.localScale = new Vector3(currentSizeX, currentSizeY, transform.localScale.z);
        }

        public override void RunEntity()
        {
            float growSpeed = 2.0f;
            if (currentSizeX < finalSizeX)
            {
                currentRaycastRadius += growSpeed * Time.deltaTime;
                currentSizeX += growSpeed * Time.deltaTime;
                currentSizeY += growSpeed * Time.deltaTime;
                transform.localScale = new Vector3(currentSizeX, currentSizeY, transform.localScale.z);
            }
            RaycastHit2D hit = (Physics2D.CircleCast(transform.position, currentRaycastRadius, Vector2.zero, 0, ~rayCastLayer));
            if (hit)
            {
                if (hit.collider.tag == targetTag)
                    hit.collider.gameObject.GetComponent<Managers.Entity>().HitByEntity(this);
                Die();
            }
            if ((currentLifeTime += Time.deltaTime) > lifeTime)
                Die();
        }

        public override void HitByEntity(Entity col)
        {
        }
    }
}
