using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    class PlayerManager : EntityManager
    {
        [SerializeField]
        private Transform spawnPoint;

        private Player.Player player;
             
        public override void Start()
        {
            if (player == null)
            {
                base.Start();
                AquireEntity(0, spawnPoint, Enums.Direction.Right);
                player = FindObjectOfType<Player.Player>();
            }
        }

        public Player.Player GetPlayer()
        {
            if (player == null)
                Start();
            return player;
        }
    }
}
