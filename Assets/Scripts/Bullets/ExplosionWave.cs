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
        private LayerMask rayCastLayer;
        [SerializeField]
        private Transform rayCastPoint;
        
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
            if (currentSizeX < finalSizeX)
            {
                currentRaycastRadius = (currentSizeX / finalSizeX);
                Debug.DrawRay(this.transform.position, Vector3.right * currentRaycastRadius, Color.red);
                currentSizeX += speed * Time.deltaTime;
                currentSizeY += speed * Time.deltaTime;
                transform.localScale = new Vector3(currentSizeX, currentSizeY, transform.localScale.z);
            }
            else
                Die();
            RaycastHit2D hit = (Physics2D.CircleCast(transform.position, currentRaycastRadius, Vector2.zero, 0, ~rayCastLayer));
            if (hit)
            {
                if (hit.collider.tag == targetTag)
                    hit.collider.gameObject.GetComponent<Managers.Entity>().HitByEntity(this);
            }
        }

        public override void HitByEntity(Entity col)
        {
        }
    }
}
