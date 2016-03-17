using UnityEngine;

namespace Assets.Scripts.Util
{
    class LevelLoad : MonoBehaviour
    {
        public string level;

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "Player")
                    UnityEngine.SceneManagement.SceneManager.LoadScene(level);
            }
        }
    }
}
