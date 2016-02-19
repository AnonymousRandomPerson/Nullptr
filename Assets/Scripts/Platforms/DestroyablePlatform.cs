using UnityEngine;

namespace Assets.Scripts.Platforms
{
    class DestroyablePlatform : MonoBehaviour
    {
        internal Vector3 posToGoTo;
        private bool moveUp;
        private bool destroyed;

        void Start()
        {
            moveUp = false;
            destroyed = false;
        }

        void Update()
        {

        }

        public void DestroyThis()
        {
            if (destroyed)
                GoToPos();
            moveUp = true;
        }

        private void GoToPos()
        {
            destroyed = false;
            moveUp = false;
            transform.position = posToGoTo;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "Enemy")
                {
                    gameObject.layer = LayerMask.NameToLayer("Default");
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    destroyed = true;
                }
            }
        }

    }
}
