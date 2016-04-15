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
        private string[] targetTags;
        [SerializeField]
        private LayerMask rayCastLayer;
        [SerializeField]
        private Transform rayCastPoint;
        [SerializeField]
        private Util.SoundPlayer sound;

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
            if (finalSizeX == 0)
            {
                finalSizeX = transform.localScale.x;
                finalSizeY = transform.localScale.y;
            }
            transform.localScale = new Vector3(currentSizeX, currentSizeY, transform.localScale.z);
            sound.PlaySong(0);
        }

        public override void RunEntity()
        {
            if (currentSizeX < finalSizeX)
            {
                currentRaycastRadius = (currentSizeX / finalSizeX);
                currentSizeX += speed * Time.deltaTime;
                currentSizeY += speed * Time.deltaTime;
                transform.localScale = new Vector3(currentSizeX, currentSizeY, transform.localScale.z);
            }
            else
                Die();
            RaycastHit2D hit = (Physics2D.CircleCast(transform.position, currentRaycastRadius, Vector2.zero, 0, ~rayCastLayer));
            if (hit)
            {
                foreach (string targetTag in targetTags)
                {
                    if (hit.collider.tag == targetTag)
                    {
                        hit.collider.gameObject.GetComponent<Managers.Entity>().HitByEntity(this);
                        break;
                    }
                }
            }
        }

        public override void HitByEntity(Entity col)
        {
        }
    }
}
