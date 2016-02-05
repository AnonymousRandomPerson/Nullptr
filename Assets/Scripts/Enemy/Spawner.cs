using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;

namespace Assets.Scripts.Enemy
{
    class Spawner : MonoBehaviour, Callback
    {
        /// <summary> Left bound for the target range. </summary>
        [SerializeField]
        private Transform leftBound;
        /// <summary> Right bound for the target range. </summary>
        [SerializeField]
        private Transform rightBound;
        /// <summary> Spawn point for enemies. </summary>
        [SerializeField]
        private Transform spawnPoint;
        /// <summary> The time to wait between each spawn</summary>
        [SerializeField]
        private float SpawnWaitTime;
        /// <summary> The specific enemy type to spawn from the enemy manager. Each entry account for one spawned enemy. </summary>
        [SerializeField]
        private int[] enemiesToSpawn;
        /// <summary> Direction to send enemies in. </summary>
        [SerializeField]
        private Enums.Direction direction;
        /// <summary> If true the spawner will continue spawning things forever. </summary>
        [SerializeField]
        private bool spawnInfinitely;

        /// <summary> Reference to EnemyManager to spawn things. </summary>
        private EnemyManager manager;
        /// <summary> The GameObject that must be in range to start spawning. </summary>
        private GameObject target;
        /// <summary> Time waited so far. </summary>
        private float timeWaited;
        /// <summary> Pointer to current enemy in enemiesToSpawn array. </summary>
        private int currentEnemy;
        /// <summary> Current number of spawned enemies. </summary>
        private int numOfEnemies;
        /// <summary> Number of times the manager couldn't spawn the requested enemy. </summary>
        private int failedAttempts;

        void Start()
        {
            manager = FindObjectOfType<EnemyManager>();
            target = FindObjectOfType<Managers.PlayerManager>().GetPlayer().gameObject;
            timeWaited = 0f;
            currentEnemy = 0;
            numOfEnemies = 0;
            failedAttempts = 0;
        }

        void Update()
        {
            if(currentEnemy < enemiesToSpawn.Length && GameManager.IsRunning && (target.transform.position.x > leftBound.position.x && target.transform.position.x < rightBound.position.x))
            {
                if(numOfEnemies < enemiesToSpawn.Length && (timeWaited += Time.deltaTime) > SpawnWaitTime)
                {
                    timeWaited = 0f;
                    if(manager.SpawnEnemy(enemiesToSpawn[currentEnemy], spawnPoint, direction, this))
                    {
                        numOfEnemies++;
                        currentEnemy++;
                        if (spawnInfinitely && currentEnemy >= enemiesToSpawn.Length)
                            currentEnemy = 0;
                    }
                    else
                        timeWaited -= Random.Range(1, 5) + (1 << failedAttempts++);
                }
            }
        }

        public void entityDied(Entity entity)
        {
            numOfEnemies--;
        }
    }
}
