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
        private GameObject characterHolderPrefab;
        [SerializeField]
        private int backgroundCount = 5;
        [SerializeField]
        private string message;
        public string Message { set { message = value; } }

        GameObject primary;
        GameObject[] secondaries;

        const float DAMPENING = 4f; //Dampening will decrease the distance background CharacterHolders appears from the primary one
        const float RANGE = 20; //Range affects the randomness of the background CharacterHolders appearing
        Vector3 initialPosition;
        Vector3[] initialPositions; //Store original positions of the BackgroundCharacterHolders so they don't run away

        void Start()
        {
            primary = Instantiate(characterHolderPrefab);
            primary.GetComponent<TextWriter>().FullText = message;
            initialPosition = primary.transform.transform.localPosition;
            initialPositions = new Vector3[backgroundCount];
            secondaries = new GameObject[backgroundCount];
            for (int i = 0; i < initialPositions.Length; i++)
            {
                secondaries[i] = Instantiate(characterHolderPrefab);
                TextWriter t = secondaries[i].GetComponent<TextWriter>();
                t.SoundEnabled = false;
                t.FullText = message;
                t.CharacterAlpha = Random.Range(.1f, .5f);
                initialPositions[i] = secondaries[i].transform.localPosition;
                t.gameObject.transform.SetParent(gameObject.transform);
            }
            primary.gameObject.transform.SetParent(gameObject.transform);
        }

        void Update()
        {
            if (!Managers.GameManager.IsRunning)
                return;
            if (primary != null)
            {
                for (int i = 0; i < secondaries.Length; i++)
                {
                    Vector3 initial = initialPositions[i];
                    float x = initial.x;
                    float y = initial.y;
                    float z = initial.z;
                    secondaries[i].transform.localPosition = new Vector3(x + Mathf.Sin(Random.Range(-RANGE, RANGE)) / DAMPENING, y + Mathf.Sin(Random.Range(-RANGE, RANGE)) / DAMPENING, z);
                }
                primary.transform.localPosition = new Vector3(initialPosition.x + Mathf.Sin(Random.Range(-RANGE, RANGE) / 1000),
                                                              initialPosition.y + Mathf.Sin(Random.Range(-RANGE, RANGE) / 1000),
                                                              initialPosition.z);
            } else {
                Destroy(this.gameObject);
            }
        }
    }
}
