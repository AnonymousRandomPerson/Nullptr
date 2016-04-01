using UnityEngine;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Enemy
{
    class BossSpawner : MonoBehaviour, Callback
    {
        [SerializeField]
        private GameObject WinText;
        [SerializeField]
        private Transform[] SpawnPoints;
        [SerializeField]
        private float timeForName;
        [SerializeField]
        private float cutSceneWait;
        [SerializeField]
        private GameObject cutScene;
        [SerializeField]
        private bool mf;

        /// <summary> Reference to EnemyManager to spawn things. </summary>
        private EnemyManager manager;
        private bool spawned, waitForCutScene;
        private int count;

        void Start()
        {
            manager = FindObjectOfType<EnemyManager>();
            spawned = false;
            waitForCutScene = false;
            count = 0;
        }

        void Update()
        {
            if(!spawned && (timeForName -= Time.deltaTime) < 0)
            {
                for(int i = 0; i < SpawnPoints.Length; i++)
                    manager.SpawnEnemy(i, SpawnPoints[i], Util.Enums.Direction.Right, this);
                if(mf)
                {
                    Boss.MallocAndFree[] p = FindObjectsOfType<Boss.MallocAndFree>() as Boss.MallocAndFree[];
                    p[0].Partner = p[1];
                    p[1].Partner = p[0];
                }
                spawned = true;
            }
            if (waitForCutScene)
            {
                if ((cutSceneWait -= Time.deltaTime) < 0)
                {
                    WinText.SetActive(false);
                    cutScene.SetActive(true);
                    waitForCutScene = false;
                }
            }
        }

        public void entityDied(Entity entity)
        {
            if(mf)
            {
                count++;
                if (count < 2)
                    return;
            }
            WinText.SetActive(true);
            GameManager.Menu();
            waitForCutScene = true;
        }
    }
}
