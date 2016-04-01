using UnityEngine;

namespace Assets.Scripts.Effects
{
    class Translate : MonoBehaviour
    {
        /// <summary> The first point to oscillate  between.</summary>
        [SerializeField]
        private Transform pointA;
        /// <summary> The second point to oscillate  between.</summary>
        [SerializeField]
        private Transform pointB;
        /// <summary> The speed to oscillate  at.</summary>
        [SerializeField]
        private float speed = 10f;
        [SerializeField]
        private Sprite[] sprites;

        /// <summary> The direction to travel in.</summary>
        private bool direction = false;
        /// <summary> The objects current location between the two points.</summary>
        private float currentPoint = 0;

        void Start()
        {
        }

        void Update()
        {
            if (!Managers.GameManager.IsRunning)
                return;
            if (direction)
                currentPoint += Time.deltaTime * speed;
            else
                currentPoint -= Time.deltaTime * speed;
            if (currentPoint > 1f)
            {
                direction = !direction;
                currentPoint = 1f;
                GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
            }
            if (currentPoint < 0f)
            {
                direction = !direction;
                currentPoint = 0f;
                GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
            }
            transform.position = Vector3.Lerp(pointA.position, pointB.position, currentPoint);
        }

        public bool Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public float CurrentPoint
        {
            get { return currentPoint; }
            set { currentPoint = value; }
        }
    }
}
