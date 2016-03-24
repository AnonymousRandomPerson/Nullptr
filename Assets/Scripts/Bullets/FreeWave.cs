using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Bullets
{
    public class FreeWave : Entity, Bullet
    {
        [SerializeField]
        private int damage;
        [SerializeField]
        private float speed;
        private Malloc mal;
        private Vector3 start;
        private float percent;

        public int getDamage()
        {
            return damage;
        }

        public override void InitData()
        {
            if (MallocManager.instance != null && MallocManager.instance.GetMallocs().Count > 0)
            {
                mal = ((Malloc)MallocManager.instance.GetMallocs()[0]);
                mal.Dying();
                MallocManager.instance.RemoveMalloc(mal);
                start = transform.position;
                percent = 0f;
            }
            else
                Die();
        }

        public override void RunEntity()
        {
            if (mal == null || !mal.enabled)
                Die();
            else
            {
                percent += speed * Time.deltaTime;
                transform.position = Vector3.Lerp(start, mal.transform.position, percent);
                if (percent >= 1)
                {
                    mal.Explode();
                    Die();
                }
            }
        }

        public override void HitByEntity(Entity col)
        {
        }
    }
}
