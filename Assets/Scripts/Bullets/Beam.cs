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
        [Tooltip("The amount of damage dealt by the beam.")]
        private int damage;
        /// <summary> The life time of the beam. </summary>
        [SerializeField]
        [Tooltip("The life time of the beam.")]
        private float lifeTime;
        /// <summary> The amount of time left for the beam to exist. </summary>
        private float currentLifeTime;

        /// <summary>
        /// Method to allow custom data initialization.
        /// </summary>
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

        /// <summary>
        /// Gets the damage dealt by the beam.
        /// </summary>
        /// <returns>The damage dealt by the beam.</returns>
        public int getDamage()
        {
            return damage;
        }

        /// <summary>
        /// Entity Update Method. Replaces Update().
        /// </summary>
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

        /// <summary>
        /// Called when a collision is detected by another entity.
        /// </summary>
        /// <param name="col">The entity colliding with this one.</param>
        public override void HitByEntity(Entity col)
        {
        }
    }
}
