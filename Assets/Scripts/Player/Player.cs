using UnityEngine;
using Assets.Scripts.Bullets;
using Assets.Scripts.Enemy;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;

namespace Assets.Scripts.Player
{
    class Player : Entity
    {
        /// <summary> The current selected weapon. </summary>
        [SerializeField]
        private WeaponSelection weapons;
        public string WeaponName { get { return weapons.GetWeapon().ToString(); } }
        /// <summary> Reference to gun. </summary>
        [SerializeField]
        private GameObject gun;
        /// <summary> Reference to the rigidbody. </summary>
        [SerializeField]
        private Rigidbody2D rgby;
        /// <summary> Reference to gun. </summary>
        [SerializeField]
        private Transform barrel;
        /// <summary> Location of the left edge of the collider for use in raycasting to the ground. </summary>
        [SerializeField]
        private Transform backFoot;
        /// <summary> Location of the right edge of the collider for use in raycasting to the ground. </summary>
        [SerializeField]
        private Transform frontFoot;
        /// <summary> Location of the bottom center of the collider for use in raycasting to the ground. </summary>
        [SerializeField]
        private Transform center;
        /// <summary> The front side of the collider for raycasting to detect something in the way. </summary>
        [SerializeField]
        private Transform front;
        /// <summary> The layers to ignore when raycasting (Bullets, Destroyed, Player). </summary>
        [SerializeField]
        private LayerMask raycastLayers;
        /// <summary> How long the enemy is invunerable after being hit. </summary>
        [SerializeField]
        private float invulerabilityTime = 1f;
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

        /// <summary> Reference to the BulletManager for shooting. </summary>
        private BulletManager bulletManager;
        /// <summary> reference to camera for finding screen point</summary>
        new private Camera camera;
        /// <summary> When greater than 0, this enemy is invunerable and takes damage. </summary>
        private float invulerability;
        /// <summary> X part of the velocity vector. </summary>
        private float xVel;
        /// <summary> Y part of the velocity vector. </summary>
        private float yVel;
        /// <summary> When true this enemy's sprite is being rendered. </summary>
        private bool render;
        /// <summary> True if the player has been hit by something damaging. </summary>
        protected bool hit;

        /// <summary> The distance from the center to the collider's side. </summary>
        private Vector3 colliderSideOffset;

        /// <summary> The height where the player dies if he drops below it. </summary>
        [SerializeField]
        [Tooltip("The height where the player dies if he drops below it.")]
        private float deathHeight = -6;

        /// <summary> The renderer for the player's body. </summary>
        private SpriteRenderer bodyRenderer;
        /// <summary> The renderer for the player's gun. </summary>
        private SpriteRenderer gunRenderer;

        /// <summary> The animator for the player sprite. </summary>
        private Animator animator;
        private int damage;

        /// <summary> The height of the player. </summary>
        private float height;

        public override void InitData()
        {
            hit = false;
            render = true;
            invulerability = 0f;
            currentHealth = totalHealth;
            bulletManager = FindObjectOfType<BulletManager>();
            weapons.GetWeapons();
            camera = FindObjectOfType<Camera>();
            animator = GetComponent<Animator>();
            bodyRenderer = GetComponent<SpriteRenderer>();
            gunRenderer = transform.FindChild("Gun").GetComponent<SpriteRenderer>();
            colliderSideOffset = front.localPosition;
            height = GetComponent<BoxCollider2D>().bounds.extents.y * 2;
        }

        public override void RunEntity()
        {
            if (hit)
            {
                if (invulerability <= 0)
                {
                    currentHealth-= damage;
                    invulerability = invulerabilityTime;
                }
                hit = false;
                damage = 0;
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
            if (currentHealth <= 0 || transform.position.y < deathHeight)
            {
                Die();
            }
            bool inAir = false;
            float groundDistance = Mathf.Infinity;
            TouchingSomething(ref inAir, ref groundDistance);
            Move(ref inAir, groundDistance);
            Aim();
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.SwitchLeft))
                weapons.SwitchLeft();
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.SwitchRight))
                weapons.SwitchRight();
            if (CustomInput.BoolFreshPress(CustomInput.UserInput.Attack))
                bulletManager.Shoot(weapons.GetWeapon(), barrel, transform.localScale.x < 0 ? Enums.Direction.Left : Enums.Direction.Right);
        }

        /// <summary> Controls player movement. </summary>
        /// <param name="inAir"> Boolean for if the player is currently in the air. </param>
        /// <param name="groundDistance"> The distance from the player to the ground if the player will hit the ground on the current tick. </param>
        private void Move(ref bool inAir, float groundDistance)
        {
            if (!animator.isInitialized)
            {
                return;
            }
            rgby.velocity = Vector2.zero;
            if (CustomInput.BoolHeld(CustomInput.UserInput.Left))
            {
                xVel = -moveSpeed;
                animator.SetBool("Walking", true);
            }
            else if (CustomInput.BoolHeld(CustomInput.UserInput.Right))
            {
                xVel = moveSpeed;
                animator.SetBool("Walking", true);
            }
            else
            {
                xVel = 0;
                animator.SetBool("Walking", false);
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

            if (xVel != 0)
            {
                bool blocked = false;
                RaycastHit2D ray;
                if (xVel > 0)
                {
                    ray = Physics2D.Raycast(transform.position + colliderSideOffset, Vector2.right, xVel * Time.deltaTime, ~raycastLayers);
                }
                else
                {
                    Vector2 sidePosition = new Vector2(transform.position.x - colliderSideOffset.x, transform.position.y + colliderSideOffset.y);
                    ray = Physics2D.Raycast(sidePosition, -Vector2.right, -xVel * Time.deltaTime, ~raycastLayers);
                }
                if (!ray || ray.collider == null)
                {
                    blocked = false;
                }
                else
                {
                    // Default layer.
                    blocked = ray.collider.gameObject.layer == 0;
                }
                if (blocked)
                {
                    xVel = 0;
                }
            }
            
            char slopeSide = 'n';
            float maxFallTick = -maxFallSpeed * Time.deltaTime;
            RaycastHit2D backCast = Physics2D.Raycast(backFoot.position, Vector2.down, maxFallTick, ~raycastLayers);
            RaycastHit2D frontCast = Physics2D.Raycast(frontFoot.position, Vector2.down, maxFallTick, ~raycastLayers);
            RaycastHit2D centerCast = Physics2D.Raycast(center.position, Vector2.down, maxFallTick, ~raycastLayers);
            if ((backCast ^ frontCast) && !centerCast)
            {
                slopeSide = backCast ? 'b' : 'f';
            }
            float yTick = yVel * Time.deltaTime;
            float headDistance = 0;
            if (yVel > 0) {
                // Check for hitting the ceiling when traveling up.
                Vector3 heightVector = Vector2.up * height;
                RaycastHit2D upBackCast = Physics2D.Raycast(backFoot.position + heightVector, Vector2.up, yTick, ~raycastLayers);
                RaycastHit2D upFrontCast = Physics2D.Raycast(frontFoot.position + heightVector, Vector2.up, yTick, ~raycastLayers);
                RaycastHit2D upCenterCast = Physics2D.Raycast(center.position + heightVector, Vector2.up, yTick, ~raycastLayers);
                if (upBackCast)
                {
                    headDistance = Mathf.Max(headDistance, upBackCast.distance);
                }
                if (upFrontCast)
                {
                    headDistance =  Mathf.Max(headDistance, upFrontCast.distance);
                }
                if (upCenterCast)
                {
                    headDistance =  Mathf.Max(headDistance, upCenterCast.distance);
                }
            }
            if (headDistance > 0 && yTick > headDistance) {
                yTick = headDistance;
                yVel = 0;
            }
            transform.Translate(new Vector3(xVel * Time.deltaTime, Mathf.Max(-groundDistance, yTick), 0));
            if (!inAir && slopeSide == 'n' && groundDistance < Mathf.Infinity)
            {
                // Place the player back on the floor after leaving a slope.
                transform.Translate(Vector3.down * groundDistance);
            }
            if (xVel == 0 || yVel != 0)
            {
                slopeSide = 'n';
            }
            float slopeOffset = 0;
            if (slopeSide == 'b')
            {
                // Check for going down slopes.
                backCast = Physics2D.Raycast(backFoot.position, Vector2.down, maxFallTick, ~raycastLayers);
                if (backCast)
                {
                    slopeOffset -= backCast.distance;
                }
            }
            else if (slopeSide == 'f')
            {
                frontCast = Physics2D.Raycast(frontFoot.position, Vector2.down, maxFallTick, ~raycastLayers);
                if (frontCast)
                {
                    slopeOffset -= frontCast.distance;
                }
            }
            if (slopeOffset != 0) {
                transform.Translate(Vector3.up * slopeOffset);
            }
            if (inAir)
            {
                if (yVel < maxFallSpeed)
                    yVel = maxFallSpeed;
                else if (yVel > maxJumpSpeed)
                    yVel = maxJumpSpeed;
                else
                    yVel -= gravity;
            } else {
                yVel = 0;
            }
        }

        /// <summary> Aims the gun. </summary>
        private void Aim()
        {
            float up, right;
            if (CustomInput.UsingPad)
            {
                up = CustomInput.Bool(CustomInput.UserInput.LookUp) ? CustomInput.Raw(CustomInput.UserInput.LookUp) : CustomInput.Raw(CustomInput.UserInput.LookDown);
                right = CustomInput.Bool(CustomInput.UserInput.LookRight) ? CustomInput.Raw(CustomInput.UserInput.LookRight) : CustomInput.Raw(CustomInput.UserInput.LookLeft);
            } else
            {
                Vector3 norm = (new Vector3(CustomInput.MouseX, CustomInput.MouseY, 0) - camera.WorldToScreenPoint(transform.position)).normalized;
                up = norm.y;
                right = norm.x;

            }

            if (right < 0)
                FaceLeft();
            else
                FaceRight();
            if (up == 0 && right == 0)
            {

            } else if (up == 0)
            {
                if (right < 0)
                    gun.transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    gun.transform.rotation = Quaternion.Euler(0, 0, 0);
            } else if (right == 0)
            {
                if (up < 0)
                    gun.transform.rotation = Quaternion.Euler(0, 0, 270);
                else
                    gun.transform.rotation = Quaternion.Euler(0, 0, 90);
            } else
            {
                if (right < 0)
                    gun.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * Mathf.Atan(up / right));
                else
                    gun.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan(up / right));
            }
        }

        public override void HitByEntity(Entity col)
        {
            if (col.gameObject.tag == "Enemy")
            {
                SetHit(col.gameObject.GetComponent<Enemy.Enemy>().CollideDamage);
            }
            else if (col.gameObject.tag == "EnemyBullet")
            {
                SetHit(col.gameObject.GetComponent<Bullet>().getDamage());
            }
        }

        /// <summary> Turns the sprite for this enemy on or off. </summary>
        /// <param name="render"> When true the sprite should be being rendered. </param>
        protected virtual void Render(bool render)
        {
            bodyRenderer.enabled = render;
            gunRenderer.enabled = render;
        }

        void OnCollisionStay2D(Collision2D coll)
        {
            if (GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "Enemy")
                {
                    SetHit(coll.gameObject.GetComponent<Enemy.Enemy>().CollideDamage);
                }
                else if (coll.gameObject.tag == "EnemyBullet")
                {
                    SetHit(coll.gameObject.GetComponent<Bullet>().getDamage());
                }
            }
        }

        /// <summary>
        /// Damages the player.
        /// </summary>
        /// <param name="newDamage">The damage to deal to the player.</param>
        private void SetHit(int newDamage)
        {
            hit = true;
            damage = newDamage;
        }

        /// <summary> Returns booleans about whether or not the player is touching another collider. </summary>
        /// <param name="inAir"> True if there is no ground currently beneath the enemy. </param>
        /// <param name="groundDistance"> The distance from the player to the ground if the player will hit the ground on the current tick. </param>
        protected void TouchingSomething(ref bool inAir, ref float groundDistance)
        {
            RaycastHit2D backCast = Physics2D.Raycast(backFoot.position, -Vector2.up, -maxFallSpeed * Time.deltaTime, ~raycastLayers);
            RaycastHit2D frontCast = Physics2D.Raycast(frontFoot.position, -Vector2.up, -maxFallSpeed * Time.deltaTime, ~raycastLayers);
            RaycastHit2D centerCast = Physics2D.Raycast(center.position, -Vector2.up, -maxFallSpeed * Time.deltaTime, ~raycastLayers);
            inAir = !(backCast || frontCast);
            if (!inAir)
            {
                if (backCast)
                {
                    groundDistance = backCast.distance;
                }
                if (frontCast)
                {
                    groundDistance = Mathf.Min(groundDistance, frontCast.distance);
                }
                if (centerCast)
                {
                    groundDistance = Mathf.Min(groundDistance, centerCast.distance);
                }
            }
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

        internal override void Die()
        {
            base.Die();
        }
    }
}
