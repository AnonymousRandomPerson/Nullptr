using UnityEngine;
using Assets.Scripts.Util;
using System.Xml;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        /// <summary> Internal instance to make sure this class is a singlton. </summary>
        public static GameManager instance;

        /// <summary> The current state of the game. </summary>
        private static Enums.GameStates state;
        /// <summary> The state to return to after a pause. </summary>
        private static Enums.GameStates prevState;
        /// <summary> Holds the current music volume. </summary>
        private static float musicVol;
        /// <summary> Holds the current sfx volume. </summary>
        private static float sfxVol;
        /// <summary> Holds all of the weapons the player has unlocked. </summary>
        private Enums.BulletTypes[] weapons;
        /// <summary> The file to save the bindings to. </summary>
        private const string filename = "sa.ve";

        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                instance = this;
                state = Enums.GameStates.Running;
                prevState = Enums.GameStates.Running;
                musicVol = .3f;
                sfxVol = .3f;
                if (FileExists())
                    Load();
                else
                {
                    //weapons = new Enums.BulletTypes[] { Enums.BulletTypes.Pistol, Enums.BulletTypes.Destroyer, Enums.BulletTypes.Malloc, Enums.BulletTypes.Free, Enums.BulletTypes.Launcher };
                    weapons = new Enums.BulletTypes[] { Enums.BulletTypes.Pistol };
                    Store();
                }

            }
            else if (this != instance)
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary> Change to Run state. </summary>
        public static void Run()
        {
            prevState = state;
            state = Enums.GameStates.Running;
        }

        /// <summary> Change to Intro state. </summary>
        public static void Menu()
        {
            prevState = state;
            state = Enums.GameStates.Menu;
        }

        /// <summary> Change to Intro state. </summary>
        public static void CutScene()
        {
            prevState = state;
            state = Enums.GameStates.CutScene;
        }

        /// <summary> Gets the current state. </summary>
        public static Enums.GameStates State
        {
            get { return state; }
        }

        /// <summary> Returns true if the game is in the Runnning state. </summary>
        public static bool IsRunning
        {
            get { return state == Enums.GameStates.Running; }
        }

        /// <summary> Returns true if the game is in the Runnning state. </summary>
        public static bool IsCutScene
        {
            get { return state == Enums.GameStates.CutScene; }
        }

        /// <summary> Handles Pausing. </summary>
        public static bool Pause
        {
            get { return state == Enums.GameStates.Paused; }
            set
            {
                if (value && state != Enums.GameStates.Paused)
                {
                    prevState = state;
                    state = Enums.GameStates.Paused;
                }
                else
                    state = prevState;
            }
        }

        /// <summary> Handles Pausing. </summary>
        public static bool Dead
        {
            get { return state == Enums.GameStates.Dead; }
            set
            {
                if (value && state != Enums.GameStates.Dead)
                {
                    prevState = state;
                    state = Enums.GameStates.Dead;
                }
                else
                    state = prevState;
            }
        }

        /// <summary> Handles Music volume. </summary>
        public static float MusicVol
        {
            get { return musicVol; }
            set
            {
                musicVol = value;
                Util.SoundPlayer[] sounds = FindObjectsOfType<Util.SoundPlayer>() as Util.SoundPlayer[];
                foreach (Util.SoundPlayer s in sounds)
                    if (!s.SFX)
                        s.SetVolume(musicVol);
            }
        }

        /// <summary> Handles SFX volume. </summary>
        public static float SFXVol
        {
            get { return sfxVol; }
            set
            {
                sfxVol = value;
                Util.SoundPlayer[] sounds = FindObjectsOfType<Util.SoundPlayer>() as Util.SoundPlayer[];
                foreach (Util.SoundPlayer s in sounds)
                    if (s.SFX)
                        s.SetVolume(sfxVol);
            }
        }

        /// <summary> Gets the current weapons. </summary>
        public Enums.BulletTypes[] Weapons
        {
            get { return weapons; }
        }

        /// <summary> Adds a new weapons to the players unlock list. </summary>
        /// <param name="bullet"> The bullet type to add. </param>
        public void AddWeapon(Enums.BulletTypes bullet)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (bullet == weapons[i])
                    return;
            }
            Enums.BulletTypes[] temp = new Enums.BulletTypes[weapons.Length + 1];
            for (int i = 0; i < weapons.Length; i++)
                temp[i] = weapons[i];
            temp[weapons.Length] = bullet;
            weapons = temp;
            Store();
        }

        /// <summary> Determines if the Input bindings file exists. </summary>
        /// <returns> True if the file exists. </returns>
        public bool FileExists()
        {
            return System.IO.File.Exists(filename);
        }

        /// <summary> Loads the input bindings file from disk.  Assumes the file exists.  Any errors encounterd simply cause it to remake the file. </summary>
        public void Load()
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    reader.ReadToFollowing("Count");
                    weapons = new Enums.BulletTypes[reader.ReadElementContentAsInt()];
                    for (int i = 0; i < weapons.Length; i++)
                    {
                        reader.ReadToFollowing("Bullet" + i);
                        weapons[i] = (Enums.BulletTypes)System.Enum.Parse(typeof(Enums.BulletTypes), reader.ReadElementContentAsString());
                    }
                    reader.Close();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message + " occured and was caught");
                weapons = new Enums.BulletTypes[] { Enums.BulletTypes.Pistol };
                Store();
            }
        }

        /// <summary> Writes the current input bindings to a file on disk. </summary>
        public void Store()
        {
            XmlDocument bindings = new XmlDocument();
            XmlNode node;
            XmlElement element;
            XmlElement root = bindings.CreateElement("Save");
            bindings.InsertAfter(root, bindings.DocumentElement);
            element = bindings.CreateElement("Count");
            node = bindings.CreateTextNode("Count");
            node.Value = weapons.Length + "";
            element.AppendChild(node);
            root.AppendChild(element);
            for (int i = 0; i < weapons.Length; i++)
            {
                element = bindings.CreateElement("Bullet" + i);
                node = bindings.CreateTextNode("Bullet" + i);
                node.Value = weapons[i].ToString();
                element.AppendChild(node);
                root.AppendChild(element);
            }
            bindings.Save(filename);
        }
    }
}
