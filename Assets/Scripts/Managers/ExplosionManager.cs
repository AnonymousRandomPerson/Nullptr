using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    class ExplosionManager : EntityManager
    {
        public static ExplosionManager instance;

        public override void Start()
        {
            base.Start();
            instance = this;
        }

        public bool SpawnExplosion(int type, Transform loc, Enums.Direction direction)
        {
            return AquireEntityCutScene(type, loc, direction);
        }

        public void OnDestroy()
        {
            instance = null;
        }
    }
}
