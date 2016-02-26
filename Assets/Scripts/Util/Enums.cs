
namespace Assets.Scripts.Util
{
    public class Enums
    {
        public enum Direction { Up, Down, Left, Right, None };
        public enum PlayerState { Idle = 0, Moving, Hit, Dead, Attack, Jump };
        public enum GameStates { Intro, Running, CutScene, Paused };
        public enum BulletTypes {
            Pistol = 0, Destroyer, Malloc, Free, MachineGun, Player6, Player7, Player8,
            Enemy1, Enemy2, Enemy3, Enemy4 
        };
        public enum EnemiesLevel01 { Walkers, Flyers };
    }
}
