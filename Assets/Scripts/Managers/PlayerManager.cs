using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    class PlayerManager : EntityManager
    {
        [SerializeField]
        private Transform spawnPoint;

        public GameObject deathHandler;

        private Player.Player player;
             
        public override void Start()
        {
            if (player == null)
            {
                base.Start();
                AquireEntity(0, spawnPoint, Enums.Direction.Right, deathHandler.GetComponent<Callback>());
                player = FindObjectOfType<Player.Player>();
            }
        }

        public Player.Player GetPlayer()
        {
            if (player == null)
                Start();
            return player;
        }

        public void spawnAt(Transform spawnPoint)
        {
            AquireEntity(0, spawnPoint, Enums.Direction.Right, deathHandler.GetComponent<Callback>());
        }
    }
}
