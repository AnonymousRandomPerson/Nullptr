using UnityEngine;

namespace Assets.Scripts.Util
{
    class Rotate : MonoBehaviour
    {
        /// <summary> How fast this boss rotates back and forth. </summary>
        [SerializeField]
        private float rotationSpeed;
        /// <summary> How much the boss has rotated so far. </summary>
        private float currentRotation;
        /// <summary> Saves the current direction of rotation. </summary>
        private bool rotatingLeft;

        void Update()
        {
            if (rotatingLeft)
            {
                transform.Rotate(new Vector3(0, 0, -rotationSpeed));
                currentRotation -= rotationSpeed;
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, rotationSpeed));
                currentRotation += rotationSpeed;
            }
            if (currentRotation < -15)
                rotatingLeft = false;
            if (currentRotation > 15)
                rotatingLeft = true;
        }
    }
}
