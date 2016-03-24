using UnityEngine;

namespace Assets.Scripts.Platforms
{
    //This fan only blows upwards

    class Fan : MonoBehaviour
    {
        //Magnitude of fan's blowing force.
		[SerializeField]
        private float blowbackMagnitude;

        void applyForce (Collider2D col)
        {
            //Change this method if there are any enemies
            //that are supposed to be still, like sedentary enemies
			col.attachedRigidbody.AddForce(blowbackMagnitude * Vector2.up, ForceMode2D.Force);
        }

        void OnTriggerEnter2D (Collider2D col)
        {
            applyForce(col);
        }

        void OnTriggerStay2D (Collider2D col)
        {
			
            applyForce(col);
        }
    }
}