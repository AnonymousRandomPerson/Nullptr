using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    class ExplosionManager : EntityManager
    {
        public static ExplosionManager instance;

        void Awake()
        {
            instance = this;
        }

        public bool SpawnExplosion(int type, Transform loc, Enums.Direction direction)
        {
            return AquireEntityCutScene(type, loc, direction);
        }
    }
}
