using UnityEngine;

namespace Assets.Scripts.UI.ActivatedTextBox {
    public class PlayerDetection : MonoBehaviour {

        bool playerIsInside;
        public bool PlayerIsInside { get { return playerIsInside; } }

        void OnTriggerEnter2D(Collider2D col) {
            PlayerIsCol(col, true);
        }

        void OnTriggerExit2D(Collider2D col) {
            PlayerIsCol(col, false);
        }

        void PlayerIsCol(Collider2D col, bool playerIsInside) {
            if (col.gameObject.tag == "Player") {
                this.playerIsInside = playerIsInside;
            }
        }
    }
}
