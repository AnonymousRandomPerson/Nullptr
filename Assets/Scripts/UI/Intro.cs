using UnityEngine;

namespace Assets.Scripts.UI
{
    class Intro : MonoBehaviour
    {
        public GameObject fakeTitle, Error, StartButton;
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
            Error.SetActive(true);
            introSongs.Stop();
            introSongs.loop = true;
            introSongs.loopSong = 1;
            introSongs.PlaySong(1);
        }

        public void ErrorClick()
        {
            fakeTitle.SetActive(false);
            Error.SetActive(false);
            StartButton.SetActive(true);
            Managers.GameManager.Run();
            introSongs.Stop();
        }

        public void Play()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
        }
    }
}
