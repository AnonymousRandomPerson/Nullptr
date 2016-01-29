using UnityEngine;

namespace Assets.Scripts.UI
{
    class Intro : MonoBehaviour
    {
        public GameObject fakeTitle, Error, StartButton;

        void Start()
        {
            Managers.GameManager.Intro();
        }

        public void FakePlay()
        {
            Error.SetActive(true);
        }

        public void ErrorClick()
        {
            fakeTitle.SetActive(false);
            Error.SetActive(false);
            StartButton.SetActive(true);
            Managers.GameManager.Run();
        }

        public void Play()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Test");
        }
    }
}
