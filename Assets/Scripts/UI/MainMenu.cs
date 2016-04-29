using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainParent;
        [SerializeField]
        private GameObject mainSelected;
        [SerializeField]
        private GameObject levelsParent;
        [SerializeField]
        private GameObject levelsSelected;
        [SerializeField]
        private GameObject settingsParent;
        [SerializeField]
        private GameObject settingsSelected;
        [SerializeField]
        private GameObject creditsParent;
        [SerializeField]
        private GameObject creditsSelected;
        [SerializeField]
        private GameObject resetParent;
        [SerializeField]
        private GameObject resetSelected;

        private GameObject currentSelected;
        private bool inMain;
        private bool inCredits;

        private static bool inLevelSelect;

        void Start()
        {
            inMain = true;
			inCredits = false;
            EventSystem.current.SetSelectedGameObject(mainSelected);
            if (inLevelSelect)
                GoToLevels();
        }

        void Update()
        {
			if (inMain) {
				if (Input.GetKey (KeyCode.Escape))
					Application.Quit ();
				if (inMain && EventSystem.current.currentSelectedGameObject == null) {
					if (inCredits)
						EventSystem.current.SetSelectedGameObject (creditsSelected);
					else
						EventSystem.current.SetSelectedGameObject (mainSelected);
				}

				currentSelected = EventSystem.current.currentSelectedGameObject;

				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Up))
					Navigator.Navigate (Util.CustomInput.UserInput.Up, currentSelected);
				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Down))
					Navigator.Navigate (Util.CustomInput.UserInput.Down, currentSelected);
				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Accept))
					Navigator.CallSubmit ();
				if (inCredits && Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Cancel))
					GoToMain ();
			} 
			else if (inCredits) {
				if (Input.GetKey (KeyCode.Escape))
					GoToMain();
				
				currentSelected = EventSystem.current.currentSelectedGameObject;

				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Up))
					Navigator.Navigate (Util.CustomInput.UserInput.Up, currentSelected);
				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Down))
					Navigator.Navigate (Util.CustomInput.UserInput.Down, currentSelected);
				if (Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Accept))
					Navigator.CallSubmit ();
				if (inCredits && Util.CustomInput.BoolFreshPressDeleteOnRead (Util.CustomInput.UserInput.Cancel))
					GoToMain ();
			}
        }
        
        public void GoToMain()
        {
            inLevelSelect = false;
            inMain = true;
			inCredits = false;
            mainParent.SetActive(true);
            levelsParent.SetActive(false);
            settingsParent.SetActive(false);
            creditsParent.SetActive(false);
            resetParent.SetActive(false);
            EventSystem.current.SetSelectedGameObject(mainSelected);
        }

        public void GoToLevels()
        {
            inLevelSelect = true;
            inMain = false;
            inCredits = false;
            mainParent.SetActive(false);
            levelsParent.SetActive(true);
            settingsParent.SetActive(false);
            creditsParent.SetActive(false);
            resetParent.SetActive(false);
            EventSystem.current.SetSelectedGameObject(levelsSelected);
        }

        public void GoToCredits()
        {
            inLevelSelect = false;
            inMain = false;
            inCredits = true;
            mainParent.SetActive(false);
            levelsParent.SetActive(false);
            settingsParent.SetActive(false);
            creditsParent.SetActive(true);
            resetParent.SetActive(false);
            EventSystem.current.SetSelectedGameObject(creditsSelected);
        }

        public void GoToSettings()
        {
            inLevelSelect = false;
            inMain = false;
			inCredits = false;
            mainParent.SetActive(false);
            levelsParent.SetActive(false);
            settingsParent.SetActive(true);
            creditsParent.SetActive(false);
            resetParent.SetActive(false);
            EventSystem.current.SetSelectedGameObject(settingsSelected);
        }

        public void DeleteData()
        {
            inLevelSelect = false;
            inMain = false;
            inCredits = false;
            mainParent.SetActive(false);
            levelsParent.SetActive(false);
            settingsParent.SetActive(false);
            creditsParent.SetActive(false);
            resetParent.SetActive(true);
            EventSystem.current.SetSelectedGameObject(resetSelected);
        }
    }
}