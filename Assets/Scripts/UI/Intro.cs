using UnityEngine;

namespace Assets.Scripts.UI
{
    class Intro : MonoBehaviour
    {
        public GameObject fakeTitle, Error, StartButton, kirbyButton;
        public Util.SoundPlayer introSongs;

        void Start()
        {
            Managers.GameManager.Menu();
            introSongs.loop = true;
            introSongs.loopSong = 0;
            introSongs.PlaySong(0);
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
