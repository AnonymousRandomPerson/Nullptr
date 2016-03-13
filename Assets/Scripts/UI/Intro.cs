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
            kirbyButton.SetActive(true);
            Managers.GameManager.Run();
            introSongs.Stop();
            introSongs.loop = true;
            introSongs.loopSong = 1;
            introSongs.PlaySong(2);
        }

        public void Play()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
        }


        public void Play1()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("KirbyLevel");
        }
    }
}
