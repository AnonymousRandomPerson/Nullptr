using UnityEngine;
using Assets.Scripts.Util;

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

        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                instance = this;
                state = Enums.GameStates.Running;
                prevState = Enums.GameStates.Running;
                musicVol = .5f;
                sfxVol = .5f;
                weapons = new Enums.BulletTypes[] { Enums.BulletTypes.Player };
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
        public static void Intro()
        {
            prevState = state;
            state = Enums.GameStates.Intro;
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
            get { return musicVol; }
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
            Enums.BulletTypes[] temp = new Enums.BulletTypes[weapons.Length + 1];
            for (int i = 0; i < weapons.Length; i++)
                temp[i] = weapons[i];
            temp[weapons.Length] = bullet;
            weapons = temp;
        }
    }
}
