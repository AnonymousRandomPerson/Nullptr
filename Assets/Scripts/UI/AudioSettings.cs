using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Menu {
	public class AudioSettings : MonoBehaviour {
		[SerializeField]
		private Text[] volumeLevel;
		[SerializeField]
		private Text[] musicLevel;
		[SerializeField]
		private GameObject backButton;

        private GameObject currentSelected;

        void Start()
        {
			ChangeVolume ((int)(Managers.GameManager.SFXVol*10));
			ChangeMusicVolume ((int)(Managers.GameManager.MusicVol*10));
            EventSystem.current.SetSelectedGameObject(backButton);
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(backButton);

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
                EventSystem.current.SetSelectedGameObject(backButton);
                Navigator.CallSubmit();
            }
        }

        public void ChangeVolume(int level) {
			float change = level / 10f;
            Managers.GameManager.SFXVol = change;

			for (int i = 0; i <= level; i++) {
				volumeLevel [i].text = "1";
			}

			for (int i = level + 1; i <= 10; i++) {	
				volumeLevel [i].text = "0";
			}
		}

		public void ChangeMusicVolume(int level) {
			float change = level / 10f;
            Managers.GameManager.MusicVol = change;

            for (int i = 0; i <= level; i++) {
				musicLevel [i].text = "1";
			}

			for (int i = level + 1; i <= 10; i++) {	
				musicLevel [i].text = "0";
			}
		}
	}
}