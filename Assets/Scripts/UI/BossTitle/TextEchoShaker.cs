using UnityEngine;
using System.Collections;

namespace Assets.Scripts.UI.BossTitle
{
    /**
     * This script will create the effect where
     * A text is typed out with glitchy side effects
     * Inspired by that memorable scene in 'Tunnel-Fable'
     */
    class TextEchoShaker : MonoBehaviour
    {
        [SerializeField]
        private GameObject primaryCharacterHolder; //Holds the CharacterHolder who is in front and not meant to move
        [SerializeField]
        private GameObject[] backgroundCharacterHolders; //Holds the background CharacterHolders part of the echo effect

        const float DAMPENING = 3; //Dampening will decrease the distance background CharacterHolders appears from the primary one
        const float RANGE = 2; //Range affects the randomness of the background CharacterHolders appearing

        Vector3[] initialPositions; //Store original positions of the BackgroundCharacterHolders so they don't run away

        void Start()
        {
            initialPositions = new Vector3[backgroundCharacterHolders.Length];
            for (int i = 0; i < backgroundCharacterHolders.Length; i++)
            {
                initialPositions[i] = backgroundCharacterHolders[i].transform.localPosition;
            }
        }

        void Update()
        {
            if (primaryCharacterHolder != null)
            {
                for (int i = 0; i < backgroundCharacterHolders.Length; i++)
                {
                    Vector3 initial = initialPositions[i];
                    float x = initial.x;
                    float y = initial.y;
                    float z = initial.z;
                    backgroundCharacterHolders[i].transform.localPosition = new Vector3(x + Mathf.Sin(Random.Range(-RANGE, RANGE)) / DAMPENING, y + Mathf.Sin(Random.Range(-RANGE, RANGE)) / DAMPENING, z);
                }
            } else {
                Destroy(this.gameObject);
            }
        }
    }
}
