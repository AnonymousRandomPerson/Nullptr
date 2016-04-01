using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    class Pause : MonoBehaviour
    {
        [SerializeField]
        private GameObject canvas;
        [SerializeField]
        private GameObject resume;
        [SerializeField]
        private GameObject quit;

        private GameObject currentSelected;

        void Start()
        {
            EventSystem.current.SetSelectedGameObject(resume);
        }

        void Update()
        {
            if (Managers.GameManager.State == Util.Enums.GameStates.Menu)
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
                    EventSystem.current.SetSelectedGameObject(resume);
                    Navigator.CallSubmit();
                }
            }
            else
            {
                if (Util.CustomInput.BoolFreshPress(Util.CustomInput.UserInput.Pause))
                {
                    Managers.GameManager.Menu();
                    canvas.SetActive(true);
                }
            }
        }

        public void Resume()
        {
            Managers.GameManager.Run();
            canvas.SetActive(false);
        }

        public void Quit()
        {
            Managers.GameManager.Menu();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }
    }
}