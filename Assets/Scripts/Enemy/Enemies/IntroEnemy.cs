using UnityEngine;

namespace Assets.Scripts.Enemy.Enemies
{
    class IntroEnemy : MonoBehaviour
    {
        [SerializeField]
        private GameObject cutscene;

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "PlayerBullet")
                {
                    if (cutscene != null)
                        cutscene.SetActive(true);
                }
            }
        }
    }
}
