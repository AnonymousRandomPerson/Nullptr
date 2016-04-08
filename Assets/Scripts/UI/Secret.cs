using UnityEngine;

namespace Assets.Scripts.UI
{
    class Secret : MonoBehaviour
    {
        void Update()
        {
            if(Input.GetKey(KeyCode.Escape))
                UnityEngine.SceneManagement.SceneManager.LoadScene("IntroTitle");
        }
    }
}
