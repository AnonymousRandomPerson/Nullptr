using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    class LevelSelect : MonoBehaviour
    {
        [SerializeField]
        private GameObject startButton;
        [SerializeField]
        private GameObject cancelButton;
        [SerializeField]
        private string[] levels;
        [SerializeField]
        private GameObject KernelLevel;

        private GameObject currentSelected;

        void Start()
        {
            EventSystem.current.SetSelectedGameObject(startButton);
            if (FindObjectOfType<Managers.GameManager>().Weapons.Length < 5)
                KernelLevel.SetActive(false);
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(startButton);

            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Up))
                Navigator.Navigate(Util.CustomInput.UserInput.Up, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Down))
                Navigator.Navigate(Util.CustomInput.UserInput.Down, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Left))
                Navigator.Navigate(Util.CustomInput.UserInput.Left, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Right))
                Navigator.Navigate(Util.CustomInput.UserInput.Right, currentSelected);
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Accept))
                Navigator.CallSubmit();
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Cancel))
            {
                EventSystem.current.SetSelectedGameObject(cancelButton);
                Navigator.CallSubmit();
            }
        }

        public void Play(int i)
        {
            Managers.GameManager.Run();
            UnityEngine.SceneManagement.SceneManager.LoadScene(levels[i]);
        }

        public void Intro()
        {
            Managers.GameManager.Run();
            UnityEngine.SceneManagement.SceneManager.LoadScene("IntroTitle");
        }
    }
}