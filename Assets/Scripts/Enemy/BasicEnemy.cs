using UnityEngine;

namespace Assets.Scripts.Enemy
{
    class BasicEnemy : Enemy
    {
        [SerializeField]
        private float speed;

        public override void RunEntity()
        {
            base.RunEntity();
            transform.Translate(GetForward() * speed * Time.deltaTime);
        }
    }
}
