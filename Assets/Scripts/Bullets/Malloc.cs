using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Bullets
{
    class Malloc : Entity, Bullet
    {
        [SerializeField]
        private GameObject mallocManager;
        [SerializeField]
        private int damage;
        [SerializeField]
        private int explosion;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float raycastRadius;
        [SerializeField]
        private string targetTag;
        [SerializeField]
        private float lifeTime;
        [SerializeField]
        private LayerMask[] rayCastLayers;
        private int rayCastLayer;
        [SerializeField]
        private Transform rayCastPoint;
        [SerializeField]
        private Rigidbody2D rgby2d;
        private float currentLifeTime;
        private MallocManager malManager;
        private bool done;
        private bool dying;

        public int getDamage()
        {
            return damage;
        }

        public override void InitData()
        {
            currentLifeTime = 0;
            if (MallocManager.instance == null)
            {
                malManager = Instantiate(mallocManager).GetComponent<MallocManager>();
                malManager.Init();
            }
            else
                malManager = MallocManager.instance;
            malManager.AddMalloc(this);
            done = false;
            dying = false;
            foreach (LayerMask layerMask in rayCastLayers)
            {
                rayCastLayer = rayCastLayer | layerMask.value;
            }
        }

        public override void RunEntity()
        {
            RaycastHit2D hit = (Physics2D.CircleCast(transform.position, raycastRadius, Vector2.zero, 0, ~rayCastLayer));
            if (hit)
            {
                if (hit.collider.tag == targetTag)
                    hit.collider.gameObject.GetComponent<Managers.Entity>().HitByEntity(this);
            }
            if (!dying && (currentLifeTime += Time.deltaTime) > lifeTime)
                Die();
        }

        public override void HitByEntity(Entity col)
        {
        }

        void FixedUpdate()
        {
            if (!done)
            {
                switch (direction)
                {
                    case Enums.Direction.Up:
                        rgby2d.AddForce(transform.up * speed, ForceMode2D.Impulse);
                        break;
                    case Enums.Direction.Down:
                        Vector3 directionDown = transform.up;
                        directionDown.y = -directionDown.y;
                        rgby2d.AddForce(directionDown * speed, ForceMode2D.Impulse);
                        break;
                    case Enums.Direction.Left:
                        Vector3 directionLeft = transform.right;
                        directionLeft.x = -directionLeft.x;
                        rgby2d.AddForce(directionLeft * speed, ForceMode2D.Impulse);
                        break;
                    case Enums.Direction.Right:
                        rgby2d.AddForce(transform.right * speed, ForceMode2D.Impulse);
                        break;
                }
                done = true;
            }
        }

        internal void Explode()
        {
            Die();
        }

        internal void Dying()
        {
            dying = true;
        }

        internal override void Die()
        {
            ExplosionManager.instance.SpawnExplosion(explosion, transform, Enums.Direction.None);
            if(!dying)
                malManager.RemoveMalloc(this);
            base.Die();
        }
    }
}
