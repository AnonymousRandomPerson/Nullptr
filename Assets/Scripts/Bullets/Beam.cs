using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Bullets
{
    /// <summary>
    /// Travels in an arc around its spawn point.
    /// </summary>
    public class Beam : Managers.Entity, Bullet
    {
        /// <summary> The amount of damage dealt by the beam. </summary>
        [SerializeField]
        private int damage;
        /// <summary> The life time of the beam. </summary>
        [SerializeField]
        private float lifeTime;
        /// <summary> The amount of time left for the beam to exist. </summary>
        private float currentLifeTime;

        public override void InitData()
        {
            transform.rotation = Quaternion.identity;
            Vector3 scale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (direction == Enums.Direction.Right) {
                scale.x = -scale.x;
            }
            transform.localScale = scale;
            currentLifeTime = lifeTime;
        }

        public int getDamage()
        {
            return damage;
        }

        public override void RunEntity()
        {
            if ((currentLifeTime -= Time.deltaTime) < 0)
            {
                Die();
            }
            else
            {
                float rotateAmount = 90 * Time.deltaTime;
                if (direction == Enums.Direction.Right) {
                    rotateAmount = -rotateAmount;
                }
                transform.Rotate(0, 0, rotateAmount);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<Managers.Entity>().HitByEntity(this);
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(collision.collider.tag);
            if (collision.collider.tag == "Player")
            {
                collision.collider.gameObject.GetComponent<Managers.Entity>().HitByEntity(this);
            }
        }

        public override void HitByEntity(Entity col)
        {
        }
    }
}
