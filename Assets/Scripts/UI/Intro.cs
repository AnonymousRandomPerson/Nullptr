using UnityEngine;

namespace Assets.Scripts.UI
{
    class Intro : MonoBehaviour
    {
        public GameObject fakeTitle, Error, StartButton, kirbyButton;
        public Util.SoundPlayer introSongs;

        void Start()
        {
            Managers.GameManager.Intro();
            introSongs.loop = true;
            introSongs.loopSong = 0;
            introSongs.PlaySong(0);
        }

        public void FakePlay()
        {
            Managers.GameManager.Run();
            UnityEngine.SceneManagement.SceneManager.LoadScene("IntroLevel");
        }
    }
}
