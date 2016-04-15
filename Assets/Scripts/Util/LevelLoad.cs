using UnityEngine;

namespace Assets.Scripts.Util
{
    class LevelLoad : MonoBehaviour
    {
        public string level;
        public GameObject fadeOut;

        private bool run;
        private float currentSizeX;
        private float currentSizeY;
        private float finalSizeX;
        private float finalSizeY;

        void Start()
        {
            run = false;
            currentSizeX = 0;
            currentSizeY = 0;
            if (finalSizeX == 0)
            {
                finalSizeX = fadeOut.transform.localScale.x;
                finalSizeY = fadeOut.transform.localScale.y;
            }
            fadeOut.transform.localScale = new Vector3(currentSizeX, currentSizeY, transform.localScale.z);
        }
        
        void Update()
        {
            if (run)
            {
                if (currentSizeX < finalSizeX)
                {
                    currentSizeX += 2 * Time.deltaTime;
                    currentSizeY += 2 * Time.deltaTime;
                    fadeOut.transform.localScale = new Vector3(currentSizeX, currentSizeY, transform.localScale.z);
                }
                else
                {
                    Managers.GameManager.Run();
                    UnityEngine.SceneManagement.SceneManager.LoadScene(level);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "Player")
                {
                    run = true;
                    Transform t = FindObjectOfType<Camera>().transform;
                    fadeOut.transform.position = new Vector3(t.position.x, t.position.y, fadeOut.transform.position.z);
                    Managers.GameManager.CutScene();
                }
            }
        }
    }
}
