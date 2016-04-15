using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    class Reset : MonoBehaviour
    {
        [SerializeField]
        private GameObject cancelButton;

        private GameObject currentSelected;

        void Start()
        {
            EventSystem.current.SetSelectedGameObject(cancelButton);
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(cancelButton);

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

        public void ResetData()
        {
            FindObjectOfType<Managers.GameManager>().Reset();
            Managers.GameManager.Run();
            UnityEngine.SceneManagement.SceneManager.LoadScene("IntroTitle");
        }
    }
}