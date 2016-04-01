using UnityEngine;

namespace Assets.Scripts.Managers
{
    class PlayerDeathHandler : MonoBehaviour, Callback
    {
        public void entityDied(Entity entity)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
