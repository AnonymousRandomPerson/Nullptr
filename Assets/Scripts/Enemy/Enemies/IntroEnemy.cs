using UnityEngine;

namespace Assets.Scripts.Enemy.Enemies
{
    class IntroEnemy : MonoBehaviour
    {
        [SerializeField]
        private GameObject cutscene;

        void OnTriggerEnter2D(Collider2D coll)
        {
            Debug.Log("A");
            if (Managers.GameManager.IsRunning)
            {
                Debug.Log("B");
                if (coll.gameObject.tag == "PlayerBullet")
                {
                    Debug.Log("C");
                    if (cutscene != null)
                        cutscene.SetActive(true);
                }
            }
        }
    }
}
