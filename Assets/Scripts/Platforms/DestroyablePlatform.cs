using UnityEngine;

namespace Assets.Scripts.Platforms
{
    class DestroyablePlatform : MonoBehaviour
    {
        public InfiniteFloor floor;
        public void DestroyThis()
        {
            floor.platformDestroyed();

        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "Enemy")
                {
                    DestroyThis();
                }
            }
        }

    }
}
