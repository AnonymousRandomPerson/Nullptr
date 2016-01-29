using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private static Enums.GameStates state;
        private static Enums.GameStates prevState;
        private static float musicVol, sfxVol;

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
            }
            else if (this != instance)
            {
                Destroy(this.gameObject);
            }
        }

        public static void Run()
        {
            prevState = state;
            state = Enums.GameStates.Running;
        }

        public static void Intro()
        {
            prevState = state;
            state = Enums.GameStates.Intro;
        }

        public static Enums.GameStates State
        {
            get { return state; }
        }

        public static bool IsRunning
        {
            get { return state == Enums.GameStates.Running; }
        }

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
    }
}
