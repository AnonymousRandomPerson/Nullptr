using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Bullets
{
    public class Free : Entity, Bullet
    {
        [SerializeField]
        private int damage;

        public int getDamage()
        {
            return damage;
        }

        public override void InitData()
        {
            if(MallocManager.instance.GetMallocs().Count > 0)
                ((Malloc)MallocManager.instance.GetMallocs()[0]).Explode();
            Die();
        }

        public override void RunEntity()
        { 
        }

        public override void HitByEntity(Entity col)
        {
        }
    }
}
