using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Managers
{
    class PlayerDeathHandler : MonoBehaviour, Callback
    {
        [SerializeField]
        private PlayerManager manager;
        /// <summary> The enemy manager in the scene. </summary>
        private EnemyManager enemyManager;

        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private GameObject resume;
        [SerializeField]
        private GameObject quit;
        [SerializeField]
        private Transform[] checkpoints;

        private GameObject currentSelected;

        private int checkpoint;
        private int dead;

        void Start()
        {
            dead = 0;
            checkpoint = 0;
            enemyManager = FindObjectOfType<EnemyManager>();
        }

        void Update()
        {
            if (GameManager.IsRunning)
            {
                if (checkpoint < checkpoints.Length - 1 && transform.position.x > checkpoints[checkpoint + 1].position.x)
                    checkpoint++;
                if (dead > 0 && dead <= 2)
                {
                    Entity.Reset = true;
                    dead++;
                }
                else if (dead > 2)
                {
                    Entity.Reset = false;
                    manager.spawnAt(checkpoints[checkpoint]);
                    enemyManager.Reset();
                    dead = 0;
                }
            }
            else if(GameManager.State == Util.Enums.GameStates.Dead)
            {
                if (EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(resume);

                currentSelected = EventSystem.current.currentSelectedGameObject;

                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Up))
                    Navigator.Navigate(Util.CustomInput.UserInput.Up, currentSelected);
                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Down))
                    Navigator.Navigate(Util.CustomInput.UserInput.Down, currentSelected);
                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Accept))
                    Navigator.CallSubmit();
                if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Cancel) ||
                    Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Pause))
                {
                    EventSystem.current.SetSelectedGameObject(quit);
                    Navigator.CallSubmit();
                }
            }
        }

        public void entityDied(Entity entity)
        {
            GameManager.Dead = true;
            canvas.SetActive(true);
        }

        public void Resume()
        {
            GameManager.Dead = false;
            canvas.SetActive(false);
            if(checkpoint == 0)
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            else
                dead = 1;
        }

        public void Quit()
        {
            GameManager.Dead = false;
            GameManager.Run();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }
    }
}
