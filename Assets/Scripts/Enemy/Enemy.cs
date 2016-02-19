using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Enemy
{
    class Enemy : Entity
    {
        /// <summary> Location of the left edge of the collider for use in raycasting to the ground. </summary>
        [SerializeField]
        private Transform backFoot;
        /// <summary> Location of the right edge of the collider for use in raycasting to the ground. </summary>
        [SerializeField]
        private Transform frontFoot;
        /// <summary> The front side of the collider for raycasting to detect something in the way. </summary>
        [SerializeField]
        private Transform front;
        /// <summary> How long the enemy is invunerable after being hit. </summary>
        [SerializeField]
        private float invulerabilityTime;

        /// <summary> When greater than 0, this enemy is invunerable and doesn't take damage. </summary>
        private float invulerability;
        /// <summary> When true this enemy's sprite is being rendered. </summary>
        private bool render;
        /// <summary> True if the enemy has been hit by something damaging. </summary>
        protected bool hit;

        protected bool Invincible
        {
            get { return invulerability > 0; }
        }

        public override void InitData()
        {
            hit = false;
            render = true;
            invulerability = 0f;
            currentHealth = totalHealth;
            if (direction == Util.Enums.Direction.Left)
                FaceLeft();
            else
                FaceRight();
        }

        public override void RunEntity()
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
            } else if (!render)
            {
                render = true;
                Render(true);
            }
            if (currentHealth <= 0)
            {
                Render(true);
                Die();
            }
        }

        public override void HitByEntity(Entity col)
        {
            if (col.gameObject.tag == "PlayerBullet")
            {
                hit = true;
                HealthDisplayManager.Instance.SetRightEntity(this);
            }
        }

        /// <summary> Turns the sprite for this enemy on or off. </summary>
        /// <param name="render"> When true the sprite should be being rendered. </param>
        protected virtual void Render(bool render)
        {
            GetComponent<SpriteRenderer>().enabled = render;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            
        }

        /// <summary> Returns booleans about whether or not the enemy is touching another collider. </summary>
        /// <param name="inAir"> True if there is no ground currently beneath the enemy. </param>
        /// <param name="blocked"> True if there is something in front of the enemy. </param>
        protected void TouchingSomething(ref bool inAir, ref bool blocked)
        {
            inAir = !(Physics2D.Raycast(backFoot.position, -Vector2.up, 0.05f) || Physics2D.Raycast(frontFoot.position, -Vector2.up, 0.05f));
            RaycastHit2D ray;
            if (this.transform.localScale.x > 0)
                ray = Physics2D.Raycast(front.position, -Vector2.right, 0.05f);
            else
                ray = Physics2D.Raycast(front.position, Vector2.right, 0.05f);
            if (!ray || ray.collider == null)
                blocked = false;
            else
                blocked = ray.collider.tag.Equals("Ground") || ray.collider.tag.Equals("Untagged");
        }

        /// <summary> Switches the enemy to face left. </summary>
        protected void FaceLeft()
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }

        /// <summary> Switches the enemy to face right. </summary>
        protected void FaceRight()
        {
            this.transform.localScale = new Vector3(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }

        /// <summary> Switches the enemy to face the opposite direction. </summary>
        protected void Turn()
        {
            this.transform.localScale = new Vector3(-(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }

        /// <summary> Gets a vector facing the same direction as the enemy. </summary>
        /// <returns> A Vector2 in the same direction the enemy is facing. </returns>
        protected Vector2 GetForward()
        {
            return new Vector2(-Mathf.Sign(this.transform.localScale.x), 0);
        }
    }
}
