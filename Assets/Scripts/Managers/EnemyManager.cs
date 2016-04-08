using UnityEngine;
using Assets.Scripts.Enemy;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    class EnemyManager : EntityManager
    {
        /// <summary> Enemy spawners in the scene. </summary>
        private Spawner[] spawners;

        /// <summary>
        /// Find all enemy spawners in the scene.
        /// </summary>
        private void Start()
        {
            spawners = FindObjectsOfType<Spawner>();
            base.Start();
        }

        public bool SpawnEnemy(int type, Transform loc, Enums.Direction direction, Callback callback)
        {
            return AquireEntity(type, loc, direction, callback);
        }

        /// <summary>
        /// Reloads all enemy spawners in the scene.
        /// </summary>
        public void Reset()
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.Reset();
            }
        }
    }
}
