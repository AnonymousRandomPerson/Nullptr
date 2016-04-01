using UnityEngine;

namespace Assets.Scripts.Effects
{
    class Spin : MonoBehaviour
    {
        public float spinSpeed = 1f;
        public bool spinDirection = true;
        private int polarity = 1;

        void Start()
        {

        }

        void Update()
        {
            if (!Managers.GameManager.IsRunning)
                return;
            if (spinDirection)
            {
                polarity = 1;
            }
            else
            {
                polarity = -1;
            }

            transform.Rotate(0, 0, spinSpeed * polarity * Time.deltaTime);
        }
    }
}
