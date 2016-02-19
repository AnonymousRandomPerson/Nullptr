using UnityEngine;

namespace Assets.Scripts.Platforms
{
    class DestroyablePlatform : MonoBehaviour
    {
        public float movementSpeed;

        internal Vector3 posToGoTo;
        private bool moveUp;

        void Start()
        {
            moveUp = false;
        }

        void Update()
        {
            if(moveUp)
                transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
        }

        public void DestroyThis()
        {
            if (LayerMask.LayerToName(gameObject.layer) == "Destroyed")
                GoToPos();
            else
                moveUp = true;
        }

        private void GoToPos()
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            if(GetComponent<Collider2D>() != null)
                GetComponent<Collider2D>().enabled = true;
            moveUp = false;
            transform.position = posToGoTo;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (Managers.GameManager.IsRunning)
            {
                if (coll.gameObject.tag == "Enemy")
                {
                    if (moveUp)
                        GoToPos();
                }
            }
        }
    }
}
