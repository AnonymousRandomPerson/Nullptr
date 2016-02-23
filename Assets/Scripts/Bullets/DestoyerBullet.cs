using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Bullets
{
    class DestoyerBullet : Managers.Entity, Bullet
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

        public int getDamage()
        {
            return damage;
        }

        public override void InitData()
        {
            currentLifeTime = 0;
        }

        public override void RunEntity()
        {
            RaycastHit2D hit;
            switch (direction)
            {
                case Enums.Direction.Up:
                    transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
                    hit = (Physics2D.CircleCast(transform.position, raycastRadius, Vector2.zero, 0, ~rayCastLayer));
                    break;
                case Enums.Direction.Down:
                    Vector3 directionDown = transform.up;
                    directionDown.y = -directionDown.y;
                    transform.Translate(directionDown * speed * Time.deltaTime, Space.World);
                    hit = (Physics2D.CircleCast(transform.position, raycastRadius, Vector2.zero, 0, ~rayCastLayer));
                    break;
                case Enums.Direction.Left:
                    Vector3 directionLeft = transform.right;
                    directionLeft.x = -directionLeft.x;
                    transform.Translate(directionLeft * speed * Time.deltaTime, Space.World);
                    hit = (Physics2D.CircleCast(transform.position, raycastRadius, Vector2.zero, 0, ~rayCastLayer));
                    break;
                case Enums.Direction.Right:
                    transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
                    hit = (Physics2D.CircleCast(transform.position, raycastRadius, Vector2.zero, 0, ~rayCastLayer));
                    break;
                default:
                    hit = (Physics2D.CircleCast(transform.position, raycastRadius, Vector2.zero, 0, ~rayCastLayer));
                    break;
            }
            if (hit)
            {
                if (hit.collider.tag == targetTag)
                {
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Destroyed");
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    hit.collider.enabled = false;
                }
                if(hit.collider.gameObject.GetComponent<Managers.Entity>() != null)
                    hit.collider.gameObject.GetComponent<Managers.Entity>().HitByEntity(this);
                Die();
            }
            if ((currentLifeTime += Time.deltaTime) > lifeTime)
                Die();
        }

        public override void HitByEntity(Entity col)
        {
            Die();
        }
    }
}
