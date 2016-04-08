using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{
    class CameraTracker : MonoBehaviour
    {
        private Transform player;
        public Transform leftBound;
        public Transform rightBound;
        public Transform upperBound;
        public Transform lowerBound;

        void Start()
        {
            //camera.aspect = Data.AspectRatio;
            //if (Data.AspectRatio == 0)
            //    camera.ResetAspect();
            player = FindObjectOfType<Managers.PlayerManager>().GetPlayer().transform;
            //transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }

        void LateUpdate()
        {
            if (!Managers.GameManager.IsRunning)
                return;
            float speed = Time.deltaTime * ((Mathf.Ceil(Mathf.Abs(this.transform.position.x - player.position.x))) + 5);
            if (player.position.x > leftBound.transform.position.x && player.position.x < rightBound.position.x)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(player.position.x, this.transform.position.y, this.transform.position.z), speed);
            else if (player.position.x < leftBound.transform.position.x)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(leftBound.position.x, this.transform.position.y, this.transform.position.z), speed);
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(rightBound.position.x, this.transform.position.y, this.transform.position.z), speed);

            if (player.position.y > lowerBound.transform.position.y && player.position.y < upperBound.position.y)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(this.transform.position.x, player.position.y + 1.5f, this.transform.position.z), speed);
            else if (player.position.y < lowerBound.transform.position.y)
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(this.transform.position.x, lowerBound.position.y + 1.5f, this.transform.position.z), speed);
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position,
                    new Vector3(this.transform.position.x, upperBound.position.y + 1.5f, this.transform.position.z), speed);
        }
        public void setBounds(Transform leftBound, Transform rightBound, Transform upperBound, Transform lowerBound)
        {
            this.leftBound = leftBound;
            this.rightBound = rightBound;
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
        }

        /// <summary>
        /// Centers the camera on the player immediately.
        /// </summary>
        public void Reset()
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }
    }
}
