using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Enemy
{
    class BossSpawner : MonoBehaviour, Callback
    {
        [SerializeField]
        private GameObject Name;
        [SerializeField]
        private GameObject WinText;
        [SerializeField]
        private Transform SpawnPoint;
        [SerializeField]
        private float timeForName;

        /// <summary> Reference to EnemyManager to spawn things. </summary>
        private EnemyManager manager;
        private bool spawned;

        void Start()
        {
            manager = FindObjectOfType<EnemyManager>();
            spawned = false;
        }

        void Update()
        {
            if(!spawned && (timeForName -= Time.deltaTime) < 0)
            {
                Name.SetActive(false);
                manager.SpawnEnemy(0, SpawnPoint, Util.Enums.Direction.Right, this);
                spawned = true;
            }
        }

        public void entityDied(Entity entity)
        {
            WinText.SetActive(true);
            GameManager.Intro();
        }
    }
}
