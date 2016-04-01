using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Effects
{
    /// <summary> Randomly rotates and scales the object. </summary>
    class Glitch : MonoBehaviour
    {
        /// <summary> The initial scale of the object. </summary>
        private Vector2 initScale;

        /// <summary> Timer for changing the object's rotation and scale. </summary>
        private float changeTimer;

        /// <summary> Registers the initial scale of the object. </summary>
        private void Start()
        {
            initScale = transform.localScale;
        }
    	
    	/// <summary> Randomly rotates and scales the object every few frames. </summary>
    	private void Update()
        {
            if (!Managers.GameManager.IsRunning)
                return;
            changeTimer -= Time.deltaTime;
            if (changeTimer < 0)
            {
                changeTimer = Random.Range(0.5f, 3f);
                transform.rotation = Random.rotation;
                transform.localScale = new Vector2(GetRandomScale(initScale.x), GetRandomScale(initScale.y));
            }
    	}

        /// <summary>
        /// Gets a random number between half and double of a certain number.
        /// </summary>
        /// <param name="initScale">A random number between half and double of a certain number, negative or positive,</param>
        private float GetRandomScale(float initScale)
        {
            float multiplier = Random.Range(0.5f, 2f);
            if (Random.Range(0, 2) == 0)
            {
                multiplier = -multiplier;
            }
            return initScale * multiplier;
        }
    }
}