using UnityEngine;
using Assets.Scripts.Managers;
using Assets.Scripts.Util;

namespace Assets.Scripts.Platforms
{
    /// <summary> A block that can be destroyed by destroyer bullet or a bomb block. </summary>
    class StarBlock : MonoBehaviour
    {

        /// <summary> The time to wait before destroying the block with a bomb block. </summary>
        [SerializeField]
        [Tooltip("The time to wait before destroying the block with a bomb block.")]
        private int time;
        /// <summary> The delay before destroying the block with a bomb block. </summary>
        private float destroyTimer;
        /// <summary> Whether the block is on a timer to be destroyed by a bomb block. </summary>
        private bool countdown;
    	
    	/// <summary>
        /// Ticks the timer for destroying the block with a bomb.
        /// </summary>
    	private void Update ()
        {
            if (!Managers.GameManager.IsRunning)
                return;
            if (countdown && gameObject.activeSelf)
            {
                destroyTimer -= Time.deltaTime;
                if (destroyTimer < 0)
                {
                    SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
                    if (renderer.enabled)
                    {
                        ExplosionManager.instance.SpawnExplosion(3, transform, Enums.Direction.None);
                        gameObject.layer = LayerMask.NameToLayer("Destroyed");
                        renderer.enabled = false;
                        gameObject.GetComponent<Collider2D>().enabled = false;
                        PlayerDeathHandler.instance.AddDestroyed(gameObject);
                    }
                    countdown = false;
                }
            }
    	}

        /// <summary>
        /// Starts the countdown timer for destroying the block.
        /// </summary>
        /// <param name="timeInterval">The time multiplier for breaking the block.</param>
        internal void StartCountdown(float timeInterval)
        {
            countdown = true;
            destroyTimer = timeInterval * time;
        }
    }
}