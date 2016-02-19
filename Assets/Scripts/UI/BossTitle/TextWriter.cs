using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.UI.BossTitle
{
    /**
     * This script allows a string to be typed out letter by letter
     * On screen as a text
     */
    class TextWriter : MonoBehaviour
    {
        [SerializeField]
        private float lingerDuration = 3f; //How long the text stays up after it's done typing
        [SerializeField]
        private float secondsPerCharacter = .5f;
        [SerializeField]
        private string fullText; //The whole bazinga to type
        [SerializeField]
        private Util.SoundPlayer sound; //Sound that is played on each character
        [SerializeField]
        private bool soundEnabled = true; //Determines if a sound plays or not
        [SerializeField]
        private GameObject characterPrefab; //The specific characterPrefab we want to create
        [SerializeField]
        private float characterAlpha = 1; //Alpha of the characterPrefab

        static readonly char[] SILENT_CHARACTERS = { '_', ' ', '-' }; //When displaying these characters, don't make a sound

        // Use this for initialization
        void Start()
        {
            StartCoroutine(WriteText());
        }

        IEnumerator WriteText()
        {
            char[] chars = fullText.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                float timer = 0;
                while (i != 0 && (timer += Time.deltaTime) < secondsPerCharacter)
                {
                    yield return null;
                }
                Parent(CreateCharacter(fullText[i]), gameObject);
            }
            Destroy(this.gameObject, lingerDuration);
            yield break;
        }

        GameObject CreateCharacter(char c)
        {
            GameObject character = (GameObject)Instantiate(characterPrefab);
            if (soundEnabled && !SILENT_CHARACTERS.Contains(c))
            {
                sound.Stop();
                sound.PlaySong(0);
            }
            Text text = character.GetComponent<Text>();
            text.text = "" + c;
            text.color = new Color(text.color.r, text.color.g, text.color.b, characterAlpha);

            return character;
        }

        static void Parent(GameObject child, GameObject parent)
        {
            child.transform.SetParent(parent.transform);
        }
    }
}
