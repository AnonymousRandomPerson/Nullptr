using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    class PlayerManager : EntityManager
    {
        [SerializeField]
        private Transform spawnPoint;
             
        void Start()
        {
            AquireEntity(0, spawnPoint, Enums.Direction.Right);
        }
    }
}
