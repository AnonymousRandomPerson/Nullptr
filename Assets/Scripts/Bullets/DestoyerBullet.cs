using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Bullets
{
    class DestoyerBullet : Managers.Entity
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private float raycastLenght;
        [SerializeField]
        private string targetTag;
        [SerializeField]
        private float lifeTime;
        [SerializeField]
        private LayerMask rayCastLayer;
        [SerializeField]
        private Transform rayCastPoint;
        private float currentLifeTime;

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
                    transform.Translate(transform.up * speed * Time.deltaTime);
                    hit = (Physics2D.Raycast(rayCastPoint.position, Vector2.up, raycastLenght, ~rayCastLayer));
                    break;
                case Enums.Direction.Down:
                    transform.Translate(-transform.up * speed * Time.deltaTime);
                    hit = (Physics2D.Raycast(rayCastPoint.position, -Vector2.up, raycastLenght, ~rayCastLayer));
                    break;
                case Enums.Direction.Left:
                    transform.Translate(-transform.right * speed * Time.deltaTime);
                    hit = (Physics2D.Raycast(rayCastPoint.position, -Vector2.right, raycastLenght, ~rayCastLayer));
                    break;
                case Enums.Direction.Right:
                    transform.Translate(Vector2.right * speed * Time.deltaTime);
                    hit = (Physics2D.Raycast(rayCastPoint.position, Vector2.right, raycastLenght, ~rayCastLayer));
                    break;
                default:
                    hit = (Physics2D.Raycast(rayCastPoint.position, transform.right, raycastLenght, ~rayCastLayer));
                    break;
            }
            if (hit)
            {
                if (hit.collider.tag == targetTag)
                {
                    hit.collider.gameObject.layer = LayerMask.NameToLayer("Destroyed");
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
