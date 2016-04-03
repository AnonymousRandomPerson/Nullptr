using UnityEngine;

namespace Assets.Scripts.UI.ActivatedTextBox {
    public class ActivatedTextBox : MonoBehaviour {
        [SerializeField]
        private PlayerDetection playerDetection;
        [SerializeField]
        private GameObject textBoxes;

        void Update() {
            textBoxes.SetActive(playerDetection.PlayerIsInside);
        }
    }
}
