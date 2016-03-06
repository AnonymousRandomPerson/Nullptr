using UnityEngine;

namespace Assets.Scripts.Effects
{
    /// <summary>
    /// Causes an object to grow and shrink.
    /// </summary>
    public class Pulse : MonoBehaviour
    {

        /// <summary> The initial scale of the object. </summary>
        private Vector3 initScale;

        /// <summary> The speed that the object pulsates at. </summary>
        [SerializeField]
        [Tooltip("The speed that the object pulsates at.")]
        private float pulseTime;
        /// <summary> The amount that the object is scaled by. </summary>
        [SerializeField]
        [Tooltip("The amount that the object is scaled by.")]
        private float pulseSize;
        /// <summary> Timer for pulsing. </summary>
        private float pulseTimer;

        /// <summary>
        /// Stores the initial scale of the object.
        /// </summary>
        private void Start()
        {
            if (initScale == Vector3.zero)
            {
                initScale = transform.localScale;
            }
        }
    	
        /// <summary>
        /// Pulsates the object.
        /// </summary>
    	private void Update()
        {
            transform.localScale = initScale * (1 + pulseSize * Mathf.Sin(pulseTimer / pulseTime));
            pulseTimer += Time.deltaTime;
    	}

        /// <summary>
        /// Resets the object scale when it is disabled.
        /// </summary>
        private void OnDisable()
        {
            if (initScale != Vector3.zero)
            {
                transform.localScale = initScale;
            }
            pulseTimer = 0;
        }
    }
}