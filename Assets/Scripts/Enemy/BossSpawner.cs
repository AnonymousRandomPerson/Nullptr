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
        private GameObject[] cutScene;
        [SerializeField]
        private bool mf;
        [SerializeField]
        private bool k;
        [SerializeField]
        private bool intro;

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
            if (waitForCutScene)
            {
                if ((cutSceneWait -= Time.deltaTime) < 0)
                {
                    WinText.SetActive(false);
                    foreach(GameObject g in cutScene)
                        g.SetActive(true);
                    waitForCutScene = false;
                }
            }
            if (!GameManager.IsRunning)
                return;
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
                if (k)
                {
                    Boss.Kernel ke = FindObjectOfType<Boss.Kernel>();
                    Boss.KernelHand[] kh = FindObjectsOfType<Boss.KernelHand>() as Boss.KernelHand[];
                    ke.leftHand = kh[0];
                    ke.rightHand = kh[1];
                }
                spawned = true;
            }
        }

        public void entityDied(Entity entity)
        {
            if(intro)
                UnityEngine.SceneManagement.SceneManager.LoadScene("Secret");
            if (mf)
            {
                count++;
                if (count < 2)
                    return;
            }
            WinText.SetActive(true);
            GameManager.CutScene();
            waitForCutScene = true;
        }
    }
}
