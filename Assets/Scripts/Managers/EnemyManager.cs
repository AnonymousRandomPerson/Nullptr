using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    class EnemyManager : EntityManager
    {
        public bool SpawnEnemy(int type, Transform loc, Enums.Direction direction, Callback callback)
        {
            return AquireEntity(type, loc, direction, callback);
        }
    }
}
