using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI
{
    class Intro : MonoBehaviour
    {
        public GameObject fakeTitle, startButton;
        public Util.SoundPlayer introSongs;

        void Start()
        {
            if (FindObjectOfType<Managers.GameManager>().Weapons.Length > 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
                return;
            }
            Managers.GameManager.Menu();
            introSongs.loop = true;
            introSongs.loopSong = 0;
            introSongs.PlaySong(0);
            EventSystem.current.SetSelectedGameObject(startButton);
        }
        void Update()
        {
            if (Util.CustomInput.BoolFreshPressDeleteOnRead(Util.CustomInput.UserInput.Accept))
                Navigator.CallSubmit();
        }

        public void FakePlay()
        {
            Managers.GameManager.Run();
            UnityEngine.SceneManagement.SceneManager.LoadScene("IntroLevel");
        }
    }
}
