using UnityEngine;

namespace Assets.Scripts.Platforms
{
    /// <summary> A block that can be destroyed by destroyer bullet or a bomb block. </summary>
    public class StarBlock : MonoBehaviour {

        /// <summary> The delay before destroying the block with a bomb block. </summary>
        [SerializeField]
        [Tooltip("The delay before destroying the block with a bomb block.")]
        private float destroyTimer;
        /// <summary> Whether the block is on a timer to be destroyed by a bomb block. </summary>
        private bool countdown;
    	
    	/// <summary>
        /// Ticks the timer for destroying the block with a bomb.
        /// </summary>
    	private void Update () {
            if (countdown)
            {
                destroyTimer -= Time.deltaTime;
                if (destroyTimer < 0)
                {
                    //TODO Spawn explosion.
                    gameObject.layer = LayerMask.NameToLayer("Destroyed");
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    gameObject.GetComponent<Collider2D>().enabled = false;
                }
            }
    	}

        /// <summary>
        /// Starts the countdown timer for destroying the block.
        /// </summary>
        internal void StartCountdown()
        {
            countdown = true;
        }
    }
}