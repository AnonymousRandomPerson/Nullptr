using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;

namespace Assets.Scripts.Player
{
    class Player : Entity
    {
        /// <summary> Reference to the BulletManager for shooting. </summary>
        [SerializeField]
        private BulletManager bulletManager;
        /// <summary> Reference to gun. </summary>
        [SerializeField]
        private GameObject gun;
        /// <summary> Reference to gun. </summary>
        [SerializeField]
        private Transform barrel;
        /// <summary> Location of the left edge of the collider for use in raycasting to the ground. </summary>
        [SerializeField]
        private Transform backFoot;
        /// <summary> Location of the right edge of the collider for use in raycasting to the ground. </summary>
        [SerializeField]
        private Transform frontFoot;
        /// <summary> The front side of the collider for raycasting to detect something in the way. </summary>
        [SerializeField]
        private Transform front;
        /// <summary> The health for an enemy to start with. </summary>
        [SerializeField]
        private int health;
        /// <summary> How long the enemy is invunerable after being hit. </summary>
        [SerializeField]
        private float invulerabilityTime;
        /// <summary> Initial movement velocity. </summary>
        [SerializeField]
        private int moveSpeed = 4;
        /// <summary> Initial jump velocity. </summary>
        [SerializeField]
        private int jumpSpeed = 4;
        /// <summary> Max vertical velocity. </summary>
        [SerializeField]
        private float maxJumpSpeed = 12f;
        /// <summary> Min vertical velocity. </summary>
        [SerializeField]
        private float maxFallSpeed = -9f;
        /// <summary> Max movement velocity. </summary>
        [SerializeField]
        private float maxRunSpeed = 5f;
        /// <summary> Gravity constant for fall acceleration. </summary>
        [SerializeField]
        private float gravity = 2f;

        /// <summary> When greater than 0, this enemy is invunerable and takes damage. </summary>
        private float invulerability;
        /// <summary> X part of the velocitiy vector. </summary>
        private float xVel;
        /// <summary> Y part of the velocitiy vector. </summary>
        private float yVel;
        /// <summary> When true this enemy's sprite is being rendered. </summary>
        private bool render;

        /// <summary> The current health of this enemy. </summary>
        protected int currentHealth;
        /// <summary> True if the enemy has been hit by something damaging. </summary>
        protected bool hit;

        public override void InitData()
        {
            hit = false;
            render = true;
            invulerability = 0f;
            currentHealth = health;
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
            }
            else if (!render)
            {
                render = true;
                Render(true);
            }
            if (currentHealth < 0)
                Die();
            bool inAir = false, blocked= false;
            TouchingSomething(ref inAir, ref blocked);
            Move(ref inAir);
            Aim();
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Attack))
                bulletManager.Shoot(Enums.BulletTypes.Player, barrel, CustomInput.Bool(CustomInput.UserInput.LookLeft) ? Enums.Direction.Left : Enums.Direction.Right);

        }

        /// <summary> Controls player movement. </summary>
        /// <param name="inAir"> Boolean for if the player is currently in the air. </param>
        private void Move(ref bool inAir)
        {

            if (CustomInput.BoolHeld(CustomInput.UserInput.Left))
            {
                xVel = -moveSpeed;
                GetComponent<Animator>().SetBool("Walking", true);
            }
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Right))
            {
                xVel = moveSpeed;
                GetComponent<Animator>().SetBool("Walking", true);
            }
            else
            {
                xVel = 0;
                GetComponent<Animator>().SetBool("Walking", false);
            }

                if (!inAir && CustomInput.BoolFreshPress(CustomInput.UserInput.Jump))
            {
                yVel = jumpSpeed;
                inAir = true;
            }

            if (Mathf.Abs(xVel) > maxRunSpeed)
            {
                if (xVel > 0)
                    xVel = maxRunSpeed;
                else
                    xVel = -maxRunSpeed;
            }
            transform.Translate(new Vector3(xVel * Time.deltaTime, yVel * Time.deltaTime, 0));
            if (inAir)
            {
                if (yVel < maxFallSpeed)
                    yVel = maxFallSpeed;
                else if (yVel > maxJumpSpeed)
                    yVel = maxJumpSpeed;
                else
                    yVel -= gravity;
            }
            else
                yVel = 0;
        }

        /// <summary> Aims the gun. </summary>
        private void Aim()
        {
            float up = CustomInput.Bool(CustomInput.UserInput.LookUp) ? CustomInput.Raw(CustomInput.UserInput.LookUp) : CustomInput.Raw(CustomInput.UserInput.LookDown);
            float right = CustomInput.Bool(CustomInput.UserInput.LookRight) ? CustomInput.Raw(CustomInput.UserInput.LookRight) : CustomInput.Raw(CustomInput.UserInput.LookLeft);
            if (CustomInput.Bool(CustomInput.UserInput.LookLeft))
                FaceLeft();
            else
                FaceRight();
            if (up == 0 && right == 0)
            {

            }
            else if (up == 0)
            {
                if (CustomInput.Bool(CustomInput.UserInput.LookLeft))
                    gun.transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    gun.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (right == 0)
            {
                if (CustomInput.Bool(CustomInput.UserInput.LookDown))
                    gun.transform.rotation = Quaternion.Euler(0, 0, 270);
                else
                    gun.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                if (CustomInput.Bool(CustomInput.UserInput.LookLeft))
                    gun.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan(up / right));
                else
                    gun.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan(up / right));
            }
        }

        public override void HitByEntity(Entity col)
        {
            if (col.gameObject.tag == "Enemy")
                hit = true;
        }

        /// <summary> Turns the sprite for this enemy on or off. </summary>
        /// <param name="render"> When true the sprite should be being rendered. </param>
        protected virtual void Render(bool render)
        {
            GetComponent<SpriteRenderer>().enabled = render;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (!GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "Enemy")
                    hit = true;
            }
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

        /// <summary> Switches the player to face left. </summary>
        protected void FaceLeft()
        {
            this.transform.localScale = new Vector3(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }

        /// <summary> Switches the player to face right. </summary>
        protected void FaceRight()
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }

        /// <summary> Switches the player to face the opposite direction. </summary>
        protected void Turn()
        {
            this.transform.localScale = new Vector3(-(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }

        /// <summary> Gets a vector facing the same direction as the player. </summary>
        /// <returns> A Vector2 in the same direction the player is facing. </returns>
        protected Vector2 GetForward()
        {
            return new Vector2(-Mathf.Sign(this.transform.localScale.x), 0);
        }
    }
}
