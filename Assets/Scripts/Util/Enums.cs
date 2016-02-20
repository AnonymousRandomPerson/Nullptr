
namespace Assets.Scripts.Util
{
    public class Enums
    {
        public enum Direction { Up, Down, Left, Right, None };
        public enum PlayerState { Idle = 0, Moving, Hit, Dead, Attack, Jump };
        public enum GameStates { Intro, Running, CutScene, Paused };
        public enum BulletTypes { Pistol = 0, Enemy, Destroyer };
        public enum EnemiesLevel01 { Walkers, Flyers };
    }
}
