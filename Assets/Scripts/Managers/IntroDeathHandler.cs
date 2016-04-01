using UnityEngine;

namespace Assets.Scripts.Managers
{
    class IntroDeathHandler : MonoBehaviour, Callback
    {
        public GameObject Error;
        public Util.SoundPlayer introSongs;

        public void entityDied(Entity entity)
        {
            Managers.GameManager.Menu();
            Error.SetActive(true);
            introSongs.loop = true;
            introSongs.loopSong = 2;
            introSongs.PlaySong(2);
        }

        public void ErrorClick()
        {
            Managers.GameManager.Run();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }
    }
}
