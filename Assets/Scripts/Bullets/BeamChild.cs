using UnityEngine;
using Assets.Scripts.Managers;


namespace Assets.Scripts.Bullets
{
    /// <summary>
    /// The part of the beam that damages the player.
    /// </summary>
    public class BeamChild : MonoBehaviour {

        /// <summary>
        /// Damages the player when hit by the part of the beam.
        /// </summary>
        /// <param name="other">The collider that hit the beam.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<Managers.Entity>().HitByEntity(transform.GetComponentInParent<Entity>());
            }
        }
    }
}